using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GeekShopping.ProductAPI.Models.Base;

namespace GeekShopping.ProductAPI.Models;

[Table("tbl_product")]
public class Product : BaseEntity
{
	[Column("name")]
	[Required]
	[StringLength(150)]
	public string Name { get; set; }

	[Column("price")]
	[Required]
	[Range(1, 10000)]
	public decimal Price { get; set; }

	[Column("description")]
	[StringLength(512)]
	public string? Description { get; set; }

	[Column("category_name")]
	[StringLength(64)]
	public string? CategoryName { get; set; }

	[Column("image_url")]
	[StringLength(300)]
	public string? ImageUrl { get; set; }
}
