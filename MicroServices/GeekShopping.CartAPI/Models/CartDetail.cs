using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using AutoMapper.Configuration.Annotations;
using GeekShopping.CartAPI.Models.Base;

namespace GeekShopping.CartAPI.Models;

[Table("tbl_cart_detail")]
public class CartDetail : BaseEntity
{
	public long CartHeaderId { get; set; }

	[ForeignKey("CartHeaderId")]
	public virtual CartHeader CartHeader { get; set; }
	public long ProductId { get; set; }

	[ForeignKey("ProductId")]
	public virtual Product Product { get; set; }

	[Column("count")]
	public int Count { get; set; }
}
