using Core.Entites;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/cart")]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCart(string id)
    {
        var cart = await _cartService.GetCartAsync(id);
        return cart is null ? NotFound() : Ok(cart);
    }

    [HttpPost("{id}")]
    public async Task<IActionResult> SaveCart([FromBody] ShopingCart cart)
    {
        await _cartService.SaveCartAsync(cart);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCart(string id)
    {
        await _cartService.DeleteCartAsync(id);
        return NoContent();
    }
}
