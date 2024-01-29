using GeekShopping.Web.Models;
using GeekShopping.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeekShopping.Web.Controllers;

public class CartController : Controller
{
	private readonly ILogger<CartController> _logger;
	private readonly ICartService _cartService;
	private readonly IProductService _productService;
	private readonly ICouponService _couponService;

	public CartController(
		ILogger<CartController> logger,
		ICartService cartService,
		IProductService productService,
		ICouponService couponService)
	{
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		_cartService = cartService ?? throw new ArgumentNullException(nameof(cartService));
		_productService = productService ?? throw new ArgumentNullException(nameof(productService));
		_couponService = couponService ?? throw new ArgumentNullException(nameof(couponService));
	}

	[Authorize]
	public async Task<IActionResult> Index()
	{
		return View(await FindByUserId());
	}

	public async Task<IActionResult> Remove(long id)
	{
		var token = await HttpContext.GetTokenAsync("access_token");
		var userId = User.Claims.Where(item => item.Type == "sub")?.FirstOrDefault()?.Value;

		var response = await _cartService.RemoveItem(id, token);

		if (!response)
			return View();

		return RedirectToAction(nameof(Index));
	}

	private async Task<CartViewModel> FindByUserId()
	{
		var token = await HttpContext.GetTokenAsync("access_token");
		var userId = User.Claims.Where(item => item.Type == "sub")?.FirstOrDefault()?.Value;

		var response = await _cartService.FindByUserId(userId, token);
		if (response?.CartHeader != null)
		{
			if (!string.IsNullOrWhiteSpace(response.CartHeader.CouponCode))
			{
				var coupon = await _couponService.FindByCode(response.CartHeader.CouponCode, token);
				if (coupon?.CouponCode != null)
					response.CartHeader.DiscountAmount = coupon.DiscountAmount;
			}

			foreach (var detail in response.CartDetails)
			{
				response.CartHeader.PurchaseAmount += detail.Product.Price * detail.Count;
			}
			response.CartHeader.PurchaseAmount -= response.CartHeader.DiscountAmount;
		}

		return response;
	}

	[HttpPost]
	[ActionName("ApplyCoupon")]
	public async Task<IActionResult> ApplyCoupon(CartViewModel model)
	{
		var token = await HttpContext.GetTokenAsync("access_token");

		var response = await _cartService.ApplyCoupon(model, token);
		if (!response)
			return View();

		return RedirectToAction(nameof(Index));
	}

	[HttpPost]
	[ActionName("RemoveCoupon")]
	public async Task<IActionResult> RemoveCoupon()
	{
		var token = await HttpContext.GetTokenAsync("access_token");
		var userId = User.Claims.Where(item => item.Type == "sub")?.FirstOrDefault()?.Value;

		var response = await _cartService.RemoveCoupon(userId, token);
		if (!response)
			return View();

		return RedirectToAction(nameof(Index));
	}

	[HttpGet]
	public async Task<IActionResult> Checkout()
	{
		return View(await FindByUserId());
	}

	[HttpPost]
	public async Task<IActionResult> Checkout(CartViewModel model)
	{
		var token = await HttpContext.GetTokenAsync("access_token");

		var response = await _cartService.Checkout(model.CartHeader, token);
		if (response == null)
			return View(model);

		if (response.GetType() == typeof(string))
		{
			TempData["Error"] = response;
			return RedirectToAction(nameof(Checkout));
		}

		return RedirectToAction(nameof(Confirmation));
	}

	[HttpGet]
	public async Task<IActionResult> Confirmation()
	{
		return View();
	}
}
