using GeekShopping.MessageBus;

namespace GeekShopping.PaymentAPI.RabbitMqSender.Interfaces;

public interface IRabbitMqMessageSender
{
	void SendMessage(BaseMessage baseMessage, string queueName);
	void SendMessage(BaseMessage baseMessage);
}
