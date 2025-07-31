using Core.Entites;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class PaymentsController(IPaymentService paymentService, IUnitOfWork unit) : BaseApiController
    {
        [Authorize]
        [HttpPost("{cartId}")]
        public async Task<ActionResult<ShopingCart>> CreateOrUpdatePaymentIntent(string cartId)
        {
            var cart = await paymentService.CreateOrUpdateShoppingCartAsync(cartId);

            if (cart == null) return BadRequest("Problem with your cart");

            return Ok(cart);

        }

        [Authorize]
        [HttpGet("delivery-methods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
        {
            var deliveryMethods = await unit.Repository<DeliveryMethod>().GetAllAsync();
            return Ok(deliveryMethods);
        }
    }
}
