using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GeekShopping.Web.Models;

public class ProductViewModel
{
	public long Id { get; set; }
	public string Name { get; set; }
	public decimal Price { get; set; }
	public string Description { get; set; }
	public string CategoryName { get; set; }
	public string ImageUrl { get; set; }

	[Range(1, 100)]
	public int Count { get; set; } = 1;

	public string SubstringName()
	{
		if (Name.Length < 24)
			return Name;

		return $"{Name[..21]}...";
	}

	public string SubstringDescription()
	{
		if (Description.Length < 355)
			return Description;

		return $"{Description[..352]}...";
	}
}
