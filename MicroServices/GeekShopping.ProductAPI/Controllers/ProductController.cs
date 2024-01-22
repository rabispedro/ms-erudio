using GeekShopping.ProductAPI.Data.ValueObjects;
using GeekShopping.ProductAPI.Models;
using GeekShopping.ProductAPI.Repositories;
using GeekShopping.ProductAPI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeekShopping.ProductAPI.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ProductController : ControllerBase
{
	private readonly ILogger<ProductController> _logger;
	private readonly IProductRepository _repository;

	public ProductController(ILogger<ProductController> logger, IProductRepository repository)
	{
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		_repository = repository ?? throw new ArgumentNullException(nameof(repository));
	}

	[HttpGet]
	[ProducesResponseType(401)]
	public async Task<ActionResult<IEnumerable<ProductVO>>> FindAll()
	{
		return Ok(await _repository.FindAll());
	}

	[HttpGet("{id}")]
	[Authorize]
	[ProducesResponseType(404)]
	public async Task<ActionResult<ProductVO>> FindById(long id)
	{
		var product = await _repository.FindById(id);
		if (product.Id <= 0)
			return NotFound("Product not found");

		return Ok(product);
	}

	[HttpPost]
	[Authorize]
	[ProducesResponseType(400)]
	[ProducesResponseType(401)]
	public async Task<ActionResult<ProductVO>> Create([FromBody] ProductVO productVo)
	{
		if (productVo == null)
			return BadRequest();
		
		return Ok(await _repository.Create(productVo));
	}

	[HttpPut]
	[Authorize]
	[ProducesResponseType(400)]
	[ProducesResponseType(401)]
	public async Task<ActionResult<ProductVO>> Update([FromBody] ProductVO productVo)
	{
		if (productVo == null)
			return BadRequest();

		return Ok(await _repository.Update(productVo));
	}

	[HttpDelete("{id}")]
	[Authorize(Roles = Role.Admin)]
	[ProducesResponseType(400)]
	[ProducesResponseType(401)]
	[ProducesResponseType(403)]
	public async Task<ActionResult> Delete(long id)
	{
		var isSucceded = await _repository.Delete(id);
		if (!isSucceded)
			return BadRequest();
		
		return Ok(isSucceded);
	}
}
