using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MsErudio.Model.Base;

namespace MsErudio.Model;

[Table("tbl_book")]
public class Book : BaseEntity
{
	[Column("title")]
	public string Title { get; set; }

	[Column("author")]
	public string Author { get; set; }

	[Column("price")]
	public decimal Price { get; set; }

	[Column("launch_date")]
	public DateTime LaunchDate { get; set; }
}
