using System.ComponentModel.DataAnnotations.Schema;
using GeekShopping.CartAPI.Models.Base;

namespace GeekShopping.CartAPI.Models;

[Table("tbl_cart_header")]
public class CartHeader : BaseEntity
{
	[Column("user_id")]
	public string UserId { get; set; }

	[Column("coupon_code")]
	public string CouponCode { get; set; }
}
