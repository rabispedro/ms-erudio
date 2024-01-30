using GeekShopping.EmailSender.Messages;
using GeekShopping.EmailSender.Models;
using GeekShopping.EmailSender.Models.Contexts;
using GeekShopping.EmailSender.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.EmailSender.Repositories;

public class EmailRepository : IEmailRepository
{
	private readonly DbContextOptions<MySqlContext> _context;

	public EmailRepository(DbContextOptions<MySqlContext> context)
	{
		_context = context ?? throw new ArgumentNullException(nameof(context));
	}

	public async Task<bool> LogEmail(UpdatePaymentResultMessage message)
	{
		if (message == null)
			return false;
		
		var emailLog = new EmailLog
		{
			Email = message.Email,
			SentDate = DateTime.UtcNow,
			Log = $"Order - {message.OrderId} has been created successfully!"
		};

		await using var _db = new MySqlContext(_context);
		_db.EmailLogs.Add(emailLog);
		await _db.SaveChangesAsync();
		
		return true;
	}
}
