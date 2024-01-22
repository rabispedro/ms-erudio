using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using GeekShop.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using GeekShopping.Web.Services.Interfaces;

namespace GeekShop.Web.Controllers;

public class HomeController : Controller
{
	private readonly ILogger<HomeController> _logger;
	private readonly IProductService _productService;

	public HomeController(ILogger<HomeController> logger, IProductService productService)
	{
		_logger = logger;
		_productService = productService;
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
