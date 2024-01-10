using Microsoft.AspNetCore.Mvc;

namespace GeekShopping.ProductAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
	private readonly ILogger<ProductController> _logger;

	public ProductController(ILogger<ProductController> logger)
	{
		_logger = logger;
	}
}
