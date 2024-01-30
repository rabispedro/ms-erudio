using GeekShopping.EmailSender.Messages;

namespace GeekShopping.EmailSender.Repositories.Interfaces;

public interface IEmailRepository
{
	Task<bool> LogEmail(UpdatePaymentResultMessage message);
}
