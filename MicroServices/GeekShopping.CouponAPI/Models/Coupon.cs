using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GeekShopping.CouponAPI.Models.Base;

namespace GeekShopping.CouponAPI.Models;

[Table("tbl_coupon")]
public class Coupon : BaseEntity
{
	[Column("code")]
	[Required]
	[StringLength(150)]
	public string Code { get; set; }

	[Column("discount_amount")]
	[Required]
	public decimal DiscountAmount { get; set; }
}
