using GeekShopping.Web.Models;
using GeekShopping.Web.Services.Interfaces;
using GeekShopping.Web.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeekShopping.Web.Controllers;

public class ProductController : Controller
{
	private readonly IProductService _productService;

	public ProductController(IProductService productService)
	{
		_productService = productService ?? throw new ArgumentNullException(nameof(productService));
	}

	[Authorize]
	public async Task<IActionResult> Index()
	{
		var token = await HttpContext.GetTokenAsync("access_token");
		return View(await _productService.FindAll(token));
	}

	public async Task<IActionResult> Create()
	{
		return View();
	}

	[HttpPost]
	[Authorize]
	public async Task<IActionResult> Create(ProductModel model)
	{
		if (!ModelState.IsValid)
			return View(model);

		var token = await HttpContext.GetTokenAsync("access_token");
		var response = await _productService.Create(model, token);
		if (response == null)
			return View(model);

		return RedirectToAction(nameof(Index));
	}

	public async Task<IActionResult> Update(long id)
	{
		var token = await HttpContext.GetTokenAsync("access_token");
		var model = await _productService.FindById(id, token);
		if (model == null)
			return NotFound();
		
		return View(model);
	}

	[HttpPost]
	[Authorize]
	public async Task<IActionResult> Update(ProductModel model)
	{
		if (!ModelState.IsValid)
			return View(model);

		var token = await HttpContext.GetTokenAsync("access_token");
		var response = await _productService.Update(model, token);
		if (response == null)
			return View(model);
		
		return RedirectToAction(nameof(Index));
	}

	[Authorize]
	public async Task<IActionResult> Delete(long id)
	{
		var token = await HttpContext.GetTokenAsync("access_token");
		var model = await _productService.FindById(id, token);
		if (model == null)
			return NotFound();

		return View(model);
	}

	[HttpPost]
	[Authorize(Roles = Role.Admin)]
	public async Task<IActionResult> Delete(ProductModel model)
	{
		var token = await HttpContext.GetTokenAsync("access_token");
		var isSucceded = await _productService.Delete(model.Id, token);
		if (!isSucceded)
			return View(model);

		return RedirectToAction(nameof(Index));
	}
}
