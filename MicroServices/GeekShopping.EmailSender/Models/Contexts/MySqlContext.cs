using Microsoft.EntityFrameworkCore;

namespace GeekShopping.EmailSender.Models.Contexts;

public class MySqlContext : DbContext
{
	public MySqlContext(DbContextOptions<MySqlContext> options) : base(options) {}

	public DbSet<EmailLog> EmailLogs { get; set; }
}
