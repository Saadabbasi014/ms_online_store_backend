using Api.DTOs;
using Core.Entites.OrderAggregate;

namespace Api.Extensions
{
    public static class OrderMapingExtentions
    {
        public static OrderDto ToDto(this Order order)
        {
            return new OrderDto
            {
                Id = order.Id,
                BuyerEmail = order.BuyerEmail,
                OrderDate = order.OrderDate,
                ShippingAddress = order.ShippingAddress,
                PaymentSummary = order.PaymentSummary,
                DeliveryMethod = order.DeliveryMethod.Description,
                ShippingPrice = order.DeliveryMethod.Price,
                OrderdItems = order.OrderdItems.Select(x => x.ToDto()).ToList(),
                SubTotal = order.SubTotal,
                Status = order.Status.ToString(),
                PaymentIntentId = order.PaymentIntentId,
                Total = order.GetTotal()
            };
        }

        public static OrderItemDto ToDto(this OrderItem orderItem)
        {
            return new OrderItemDto
            {
                ProductId = orderItem.ItemOrderd.ProductId,
                ProductName = orderItem.ItemOrderd.ProductName,
                ImgUrl = orderItem.ItemOrderd.ImgUrl,
                Price = orderItem.Price,
                Quantity = orderItem.Quantity,
            };

        }
    }
}
