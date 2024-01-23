using System.Text;

namespace GeekShopping.Web.Models;

public class CartDetailViewModel
{
	public long Id { get; set; }
	public long CartHeaderId { get; set; }

	public CartHeaderViewModel CartHeader { get; set; }

	public long ProductId { get; set; }

	public ProductViewModel Product { get; set; }

	public int Count { get; set; }

	public override string ToString()
	{
		var result = new StringBuilder();
		result.Append($"[ Id: {Id},\n");
		result.Append($"CartHeaderId: {CartHeaderId},\n");
		result.Append($"CartHeader: {CartHeader},\n");
		result.Append($"ProductId: {ProductId},\n");
		result.Append($"Product: {Product},\n");
		result.Append($"Count: {Count} ]\n");

		return result.ToString();
	}
}
