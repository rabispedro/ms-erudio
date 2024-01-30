using GeekShopping.MessageBus;

namespace GeekShopping.OrderAPI.RabbitMqSender.Interfaces;

public interface IRabbitMqMessageSender
{
	void SendMessage(BaseMessage baseMessage, string queueName);
}
