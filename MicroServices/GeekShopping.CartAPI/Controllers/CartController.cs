using AutoMapper;
using GeekShopping.CartAPI.Data.ValueObjects;
using GeekShopping.CartAPI.Messages;
using GeekShopping.CartAPI.RabbitMqSender.Interfaces;
using GeekShopping.CartAPI.Repositories.Interface;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace GeekShopping.CartAPI.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class CartController : ControllerBase
{
	private readonly ICartRepository _cartRepository;
	private readonly ICouponRepository _couponRepository;
	private readonly IRabbitMqMessageSender _rabbitMqMessageSender;

	public CartController(
		ICartRepository cartRepository,
		ICouponRepository couponRepository,
		IRabbitMqMessageSender rabbitMqMessageSender)
	{
		_cartRepository = cartRepository ?? throw new ArgumentNullException(nameof(cartRepository));
		_couponRepository = couponRepository ?? throw new ArgumentNullException(nameof(couponRepository));
		_rabbitMqMessageSender = rabbitMqMessageSender ?? throw new ArgumentNullException(nameof(rabbitMqMessageSender));
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

	[HttpPost("apply-coupon/")]
	public async Task<ActionResult<bool>> ApplyCoupon([FromBody] CartVO cartVo)
	{
		var status = await _cartRepository.ApplyCoupon(cartVo.CartHeader.UserId, cartVo.CartHeader.CouponCode);
		if (!status)
			return NotFound();

		return Ok(status);
	}

	[HttpDelete("remove-coupon/{userId}")]
	public async Task<ActionResult<bool>> RemoveCoupon(string userId)
	{
		var status = await _cartRepository.RemoveCoupon(userId);
		if (!status)
			return NotFound();

		return Ok(status);
	}

	[HttpPost("checkout")]
	public async Task<ActionResult<CheckoutHeaderVO>> Checkout([FromBody] CheckoutHeaderVO checkoutHeaderVo)
	{
		if (checkoutHeaderVo?.UserId == null)
			return BadRequest();

		var cart = await _cartRepository.FindByUserId(checkoutHeaderVo.UserId);
		if (cart == null)
			return NotFound();

		if (!string.IsNullOrWhiteSpace(checkoutHeaderVo.CouponCode))
		{
			string token = await HttpContext.GetTokenAsync("access_token");
			var coupon = await _couponRepository.FindByCode(checkoutHeaderVo.CouponCode, token);

			if (checkoutHeaderVo.DiscountAmount != coupon.DiscountAmount)
				return StatusCode(412);
		}

		checkoutHeaderVo.CartDetails = cart.CartDetails;
		checkoutHeaderVo.DateTime = DateTime.UtcNow;

		_rabbitMqMessageSender.SendMessage(checkoutHeaderVo, "checkout_queue");

		await _cartRepository.Clear(checkoutHeaderVo.UserId);

		return Ok(checkoutHeaderVo);
	}
}
