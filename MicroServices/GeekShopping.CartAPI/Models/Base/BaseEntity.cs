using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GeekShopping.CartAPI.Models.Base;

public abstract class BaseEntity
{
	[Key]
	[Column("id")]
	public long Id { get; set; }
}
