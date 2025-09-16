using Api.DTOs;
using Api.Extensions;
using Api.Hubs;
using Core.Entites;
using Core.Entites.OrderAggregate;
using Core.Interfaces;
using Core.Specification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Stripe;

namespace Api.Controllers
{
    public class OrdersController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICartService _cartService;
        private readonly IHubContext<NotificationHub> _hubContext;

        public OrdersController(
            ICartService cartService,
            IUnitOfWork unitOfWork,
            IHubContext<NotificationHub> hubContext)
        {
            _unitOfWork = unitOfWork;
            _cartService = cartService;
            _hubContext = hubContext;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(CreateOrderDto orderDto)
        {
            var email = User.GetUserEmail();

            var cart = await _cartService.GetCartAsync(orderDto.CartId);

            if (cart == null) return BadRequest("Cart not found");
            if (string.IsNullOrEmpty(cart.PaymentIntentId)) return BadRequest("No payment intent for this order");

            var items = new List<OrderItem>();
            foreach (var item in cart.Items)
            {
                var productItem = await _unitOfWork.Repository<Core.Entites.Product>().GetByIdAsync(item.ProductId);

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

            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(orderDto.DeliveryMethodId);

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

            await _unitOfWork.Repository<Order>().AddAsync(order);

            await _hubContext.Clients.All.SendAsync("ReceiveOrderUpdate", order.Id, order.Status);
            await _hubContext.Clients.All.SendAsync("ReceiveNotification",$"Order {order.Id} has been {order.Status}");



            if (await _unitOfWork.Complete())
            {
                return order;
            }

            return BadRequest("Problem order creating");
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Order>>> GetOrdersForUser()
        {
            var spec = new OrderSpecification(User.GetUserEmail());

            var orders = await _unitOfWork.Repository<Order>().GetListAsync(spec);

            var orderToReturn = orders.Select(o => o.ToDto()).ToList();

            return Ok(orderToReturn);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<OrderDto>> GetOrderById(int id)
        {
            var spec = new OrderSpecification(User.GetUserEmail(), id);

            var order = await _unitOfWork.Repository<Order>().GetEntityWithSpec(spec);

            if (order == null) return NoContent();

            return order!.ToDto();
        }

        [HttpPost("update-status")]
        public async Task<IActionResult> UpdateOrderStatus(string orderId, string status)
        {
            // Your order update logic (DB update, etc.)

            // Notify connected clients
            await _hubContext.Clients.All.SendAsync("ReceiveOrderUpdate", orderId, status);

            return Ok(new { orderId, status, message = "Order status updated and notification sent." });
        }
    }
}
 