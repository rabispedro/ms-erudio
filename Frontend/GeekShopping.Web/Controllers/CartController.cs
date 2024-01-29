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

	public CartController(
		ILogger<CartController> logger,
		ICartService cartService,
		IProductService productService)
	{
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		_cartService = cartService ?? throw new ArgumentNullException(nameof(cartService));
		_productService = productService ?? throw new ArgumentNullException(nameof(productService));
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
			foreach (var detail in response.CartDetails)
			{
				response.CartHeader.PurchaseAmount += detail.Product.Price * detail.Count;
			}
		}

		return response;
	}
}
