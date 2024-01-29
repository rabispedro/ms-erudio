using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Http.Headers;
using GeekShopping.OrderAPI.Models;
using GeekShopping.OrderAPI.Models.Base;

namespace GeekShopping.OrderDetailAPI.Models;

[Table("tbl_order_detail")]
public class OrderDetail : BaseEntity
{
	public long OrderHeaderId { get; set; }

	[ForeignKey("OrderHeaderId")]
	public virtual OrderHeader OrderHeader { get; set; }

	[Column("product_id")]
	public long ProductId { get; set; }

	[Column("count")]
	public int Count { get; set; }

	[Column("product_name")]
	public string ProductName { get; set; }

	[Column("price")]
	public decimal Price { get; set; }
}
