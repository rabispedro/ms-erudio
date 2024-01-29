using GeekShopping.CouponAPI.Repositories.Interfaces;
using GeekShopping.CouponVOAPI.Data.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeekShopping.CouponAPI.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class CouponController : ControllerBase
{
	private readonly ILogger<CouponController> _logger;
	private readonly ICouponRepository _couponRepository;

	public CouponController(ILogger<CouponController> logger, ICouponRepository couponRepository)
	{
		_logger = logger;
		_couponRepository = couponRepository ?? throw new ArgumentNullException(nameof(couponRepository));
	}

	[HttpGet("{code}")]
	[Authorize]
	public async Task<ActionResult<CouponVO>> FindByCode(string code)
	{
		var coupon = await _couponRepository.FindByCode(code);
		if (coupon == null)
			return NotFound();

		return Ok(coupon);
	}
}
