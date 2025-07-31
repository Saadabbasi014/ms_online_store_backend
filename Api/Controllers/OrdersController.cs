using Api.DTOs;
using Api.Extensions;
using Core.Entites;
using Core.Entites.OrderAggregate;
using Core.Interfaces;
using Core.Specification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace Api.Controllers
{
    public class OrdersController(ICartService cartService, IUnitOfWork unitOfWork) : BaseApiController
    {
        [Authorize]
        public async Task<ActionResult<Order>> CreateOrder(CreateOrderDto orderDto)
        {
            var email = User.GetUserEmail();

            var cart = await cartService.GetCartAsync(orderDto.CartId);

            if (cart == null) return BadRequest("Cart not found");
            if (string.IsNullOrEmpty(cart.PaymentIntentId)) return BadRequest("No payment intent for this order");

            var items = new List<OrderItem>();
            foreach (var item in cart.Items)
            {
                var productItem = await unitOfWork.Repository<Core.Entites.Product>().GetByIdAsync(item.ProductId);

                if (productItem == null) return BadRequest("Problem with order");

                var itemOrderd = new ProductItemOrderd
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    ImgUrl = productItem.ImgUrl,
                };

                var orderItem = new OrderItem
                {
                    ItemOrderd = itemOrderd,
                    Price = item.Price,
                    Quantity = item.Quantity,
                };
                items.Add(orderItem);
            }

            var deliveryMethod = await unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(orderDto.DeliveryMethodId);

            if (deliveryMethod == null) return BadRequest("No delivery method selected.");

            var order = new Order
            {
                OrderdItems = items,
                DeliveryMethod = deliveryMethod,
                ShippingAddress = orderDto.ShippingAddress,
                SubTotal = items.Sum(x => x.Price * x.Quantity),
                PaymentSummary = orderDto.PaymentSummary,
                PaymentIntentId = cart.PaymentIntentId,
                BuyerEmail = email,
            };

            await unitOfWork.Repository<Order>().AddAsync(order);
            if (await unitOfWork.Complete())
            {
                return order;
            }

            return BadRequest("Problem order creating");
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Order>>> GetOrdersForUser()
        {
            var spec = new OrderSpecification(User.GetUserEmail());

            var orders = await unitOfWork.Repository<Order>().GetListAsync(spec);

            return Ok(orders);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Order>> GetOrderById(int id)
        {
            var spec = new OrderSpecification(User.GetUserEmail(), id);

            var order = await unitOfWork.Repository<Order>().GetByIdAsync(id);

            return Ok(order);
        }
    }
}
 