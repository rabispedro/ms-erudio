using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GeekShopping.CouponAPI.Models.Base;

namespace GeekShopping.CouponAPI.Models;

[Table("tbl_coupon")]
public class Coupon : BaseEntity
{
	[Column("coupon_code")]
	[Required]
	[StringLength(30)]
	public string CouponCode { get; set; }

	[Column("discount_amount")]
	[Required]
	public decimal DiscountAmount { get; set; }
}
