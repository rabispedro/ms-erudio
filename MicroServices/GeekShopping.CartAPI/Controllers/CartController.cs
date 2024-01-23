using GeekShopping.CartAPI.Data.ValueObjects;
using GeekShopping.CartAPI.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;

namespace GeekShopping.CartAPI.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class CartController : ControllerBase
{
	private readonly ICartRepository _cartRepository;

	public CartController(ICartRepository cartRepository)
	{
		_cartRepository = cartRepository ?? throw new ArgumentNullException(nameof(cartRepository));
	}

	[HttpGet("find-cart/{userId}")]
	public async Task<ActionResult<CartVO>> FindByUserId(string userId)
	{
		var cart = await _cartRepository.FindByUserId(userId);
		if (cart == null)
			return NotFound();

		return Ok(cart);
	}

	[HttpPost("add-cart")]
	public async Task<ActionResult<CartVO>> Add([FromBody] CartVO cartVo)
	{
		var cart = await _cartRepository.SaveOrUpdate(cartVo);
		if (cart == null)
			return BadRequest();
		
		return Ok(cart);

		// if (cartVo == null)
		// 	return BadRequest();

		// return Ok(await _cartRepository.SaveOrUpdate(cartVo));
	}

	[HttpPut("update-cart")]
	public async Task<ActionResult<CartVO>> Update([FromBody] CartVO cartVo)
	{
		if (cartVo == null)
			return BadRequest();

		return Ok(await _cartRepository.SaveOrUpdate(cartVo));
	}

	[HttpDelete("remove-cart/{cartDetailId}")]
	public async Task<IActionResult> RemoveCartDetail(long cartDetailId)
	{
		var isSucceded = await _cartRepository.RemoveCartDetail(cartDetailId);
		if (!isSucceded)
			return BadRequest();

		return Ok(isSucceded);
	}
}
