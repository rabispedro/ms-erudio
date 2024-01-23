using System.Text;

namespace GeekShop.Web.Models;

public class ErrorViewModel
{
	public string? RequestId { get; set; }
	public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

	public override string ToString()
	{
		var result = new StringBuilder();
		result.Append($"[ RequestId: {RequestId},\n");
		result.Append($"ShowRequestId: {ShowRequestId} ]\n");

		return result.ToString();
	}
}
