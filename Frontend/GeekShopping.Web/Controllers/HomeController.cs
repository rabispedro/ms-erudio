using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using GeekShop.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using GeekShopping.Web.Services.Interfaces;
using GeekShopping.Web.Models;
using GeekShopping.Web.Services;

namespace GeekShop.Web.Controllers;

public class HomeController : Controller
{
	private readonly ILogger<HomeController> _logger;
	private readonly IProductService _productService;
	private readonly ICartService _cartService;

	public HomeController(
		ILogger<HomeController> logger,
		IProductService productService,
		ICartService cartService)
	{
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		_productService = productService ?? throw new ArgumentNullException(nameof(productService));
		_cartService = cartService ?? throw new ArgumentNullException(nameof(cartService));
	}

	public async Task<IActionResult> Index()
	{
		return View(await _productService.FindAll(""));
	}

	[Authorize]
	public async Task<IActionResult> Details(long id)
	{
		var token = await HttpContext.GetTokenAsync("access_token");
		return View(await _productService.FindById(id, token));
	}

	[HttpPost]
	[ActionName("Details")]
	[Authorize]
	public async Task<IActionResult> DetailsPost(ProductViewModel model)
	{
		var token = await HttpContext.GetTokenAsync("access_token");

		var cart = new CartViewModel
		{
			CartHeader = new()
			{
				UserId = User.Claims.Where(item => item.Type == "sub")?.FirstOrDefault()?.Value
			},
		};
		var cartDetail = new CartDetailViewModel
		{
			Count = model.Count,
			ProductId = model.Id,
			Product = await _productService.FindById(model.Id, token)
		};

		var cartDetails = new List<CartDetailViewModel>
		{
			cartDetail
		};

		cart.CartDetails = cartDetails;
		Console.WriteLine($"UserId: {cart.CartHeader.UserId}");

		var response = await _cartService.AddItem(cart, token);
		if (response == null)
			return View(model);

		return RedirectToAction(nameof(Index));
	}

	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	public IActionResult Error()
	{
		return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
	}

	[Authorize]
	public async Task<IActionResult> Login()
	{
		var accessToken = await HttpContext.GetTokenAsync("access_token");
		return RedirectToAction(nameof(Index));
	}

	public IActionResult Logout()
	{
		return SignOut("Cookies", "oidc");
	}
}
