using Core.Entites;
using Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Stripe;
using Product = Core.Entites.Product;

namespace Infrastructure.Services
{
    public class PaymentServices(
            IConfiguration config,
            ICartService cartService,
            IUnitOfWork unit
            ) : IPaymentService
    {
        public async Task<ShopingCart?> CreateOrUpdateShoppingCartAsync(string cartId)
        {
            StripeConfiguration.ApiKey = config["StripeSettings:SecretKey"];

            var cart = cartService.GetCartAsync(cartId).Result;

            if (cart == null) return null;

            var shippingPrice = 0m;

            if (cart.DeliveryMethodId != 0)
            {
                var deliveryMethod = unit.Repository<DeliveryMethod>().GetByIdAsync(cart.DeliveryMethodId).Result;
                if (deliveryMethod != null)
                {
                    shippingPrice = deliveryMethod.Price;
                }
            }

            foreach (var item in cart.Items)
            {
                var productItem = await unit.Repository<Product>().GetByIdAsync(item.ProductId);

                if (productItem == null) continue;

                if (item.Price != productItem.Price)
                {
                    item.Price = productItem.Price;
                }
            }

            var service = new PaymentIntentService();
            PaymentIntent? paymentIntent = null;

            if (string.IsNullOrEmpty(cart.PaymentIntentId))
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long)(cart.Items.Sum(x => x.Quantity * (x.Price * 100)) 
                        + (long)shippingPrice * 100),
                    Currency = "usd",
                    PaymentMethodTypes = ["card"] 
                };
                
                paymentIntent = await service.CreateAsync(options); 
                cart.PaymentIntentId = paymentIntent.Id;
                cart.ClientSecret = paymentIntent.ClientSecret;
            }
            else
            {
                var options = new PaymentIntentUpdateOptions
                {
                    Amount = (long)(cart.Items.Sum(x => x.Quantity * (x.Price * 100))
                             + (long)shippingPrice * 100),
                };
                paymentIntent = await service.UpdateAsync(cart.PaymentIntentId, options);
                cart.ClientSecret = paymentIntent.ClientSecret;
            }

            await cartService.SaveCartAsync(cart);

            return cart;
        }
    }
}
