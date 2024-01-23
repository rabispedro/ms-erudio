using System.Text;

namespace GeekShopping.Web.Models;

public class CartHeaderViewModel
{
	public long Id { get; set; }
	public string UserId { get; set; }
	public string CouponCode { get; set; }
	public double PurchaseAmount { get; set; }

	public override string ToString()
	{
		var result = new StringBuilder();
		result.Append($"[ Id: {Id},\n");
		result.Append($"UserId: {UserId},\n");
		result.Append($"CouponCode: {CouponCode},\n");
		result.Append($"PurchaseAmount: {PurchaseAmount} ]\n");

		return result.ToString();
	}
}
