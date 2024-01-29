using Microsoft.AspNetCore.Mvc;

namespace GeekShopping.OrderAPI.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class OrderController : ControllerBase
{
	private readonly ILogger<OrderController> _logger;

	public OrderController(ILogger<OrderController> logger)
	{
		_logger = logger;
	}

}
