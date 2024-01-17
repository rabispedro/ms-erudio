using GeekShopping.Web.Models;
using GeekShopping.Web.Services.Interfaces;
using GeekShopping.Web.Utils;
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
		return View(await _productService.FindAll());
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
		
		var response = await _productService.Create(model);
		if (response == null)
			return View(model);

		return RedirectToAction(nameof(Index));
	}

	public async Task<IActionResult> Update(long id)
	{
		var model = await _productService.FindById(id);
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

		var response = await _productService.Update(model);
		if (response == null)
			return View(model);
		
		return RedirectToAction(nameof(Index));
	}

	[Authorize]
	public async Task<IActionResult> Delete(long id)
	{
		var model = await _productService.FindById(id);
		if (model == null)
			return NotFound();

		return View(model);
	}

	[HttpPost]
	[Authorize(Roles = Role.Admin)]
	public async Task<IActionResult> Delete(ProductModel model)
	{
		var isSucceded = await _productService.Delete(model.Id);
		if (!isSucceded)
			return View(model);

		return RedirectToAction(nameof(Index));
	}
}
