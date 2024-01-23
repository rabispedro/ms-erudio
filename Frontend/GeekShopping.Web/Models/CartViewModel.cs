using System.Text;

namespace GeekShopping.Web.Models;

public class CartViewModel
{
	public CartHeaderViewModel CartHeader { get; set; }
	public IEnumerable<CartDetailViewModel> CartDetails { get; set; }

	public override string ToString()
	{
		var result = new StringBuilder();
		result.Append($"[\n\tCartHeader: {CartHeader},\n");

		result.Append($"\tCartDetails: \n[ ");
		foreach (var cartDetail in CartDetails)
			result.Append($"\t\tCartDetail: {cartDetail}, \n");

		result.Append("]\n");
		return result.ToString();
	}
}
