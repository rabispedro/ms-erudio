using System.Text;
using System.Text.Json;
using GeekShopping.CartAPI.Messages;
using GeekShopping.CartAPI.RabbitMqSender.Interfaces;
using GeekShopping.MessageBus;
using RabbitMQ.Client;

namespace GeekShopping.CartAPI.RabbitMqSender;

public class RabbitMqMessageSender : IRabbitMqMessageSender
{
	private readonly string _hostName;
	private readonly string _password;
	private readonly string _username;
	private IConnection _connection;

	public RabbitMqMessageSender()
	{
		_hostName = "172.17.0.6";
		_username = "guest";
		_password = "guest";
	}
	public void SendMessage(BaseMessage baseMessage, string queueName)
	{
		var factory = new ConnectionFactory
		{
			HostName = _hostName,
			UserName = _username,
			Password = _password
		};

		_connection = factory.CreateConnection();

		using var channel = _connection.CreateModel();
		channel.QueueDeclare(queue: queueName, false, false, false, arguments: null);

		byte[] body = GetMessageAsByteArray(baseMessage);

		channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
	}

	private byte[] GetMessageAsByteArray(BaseMessage message)
	{
		var options = new JsonSerializerOptions
		{
			WriteIndented = true
		};
		var json = JsonSerializer.Serialize<CheckoutHeaderVO>((CheckoutHeaderVO) message, options);

		return Encoding.UTF8.GetBytes(json);
	}
}
