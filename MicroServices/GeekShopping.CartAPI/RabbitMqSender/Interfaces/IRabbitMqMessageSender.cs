using GeekShopping.MessageBus;

namespace GeekShopping.CartAPI.RabbitMqSender.Interfaces;

public interface IRabbitMqMessageSender
{
	void SendMessage(BaseMessage baseMessage, string queueName);
}
