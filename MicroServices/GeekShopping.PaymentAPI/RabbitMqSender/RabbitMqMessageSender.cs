using System.Text;
using System.Text.Json;
using GeekShopping.MessageBus;
using GeekShopping.PaymentAPI.Messages;
using GeekShopping.PaymentAPI.RabbitMqSender.Interfaces;
using RabbitMQ.Client;

namespace GeekShopping.PaymentAPI.RabbitMqSender;

public class RabbitMqMessageSender : IRabbitMqMessageSender
{
	private readonly string _hostName;
	private readonly string _password;
	private readonly string _username;
	private IConnection _connection;
	// private const string ExchangeName = "fanout_payment_update_exchange";
	private const string ExchangeName = "direct_payment_update_exchange";
	private const string PaymentEmailUpdateQueueName = "payment_email_update_queue";
	private const string PaymentOrderUpdateQueueName = "payment_order_update_queue";

	public RabbitMqMessageSender()
	{
		_hostName = "localhost";
		_username = "guest";
		_password = "guest";
	}
	public void SendMessage(BaseMessage baseMessage, string queueName)
	{
		if (ConnectionExists())
		{
			using var channel = _connection.CreateModel();
			channel.QueueDeclare(queue: queueName, false, false, false, arguments: null);

			byte[] body = GetMessageAsByteArray(baseMessage);

			channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
		}
	}

	public void SendMessage(BaseMessage baseMessage)
	{
		if (ConnectionExists())
		{
			using var channel = _connection.CreateModel();
			channel.ExchangeDeclare(ExchangeName, ExchangeType.Direct, durable: false);

			channel.QueueDeclare(PaymentEmailUpdateQueueName, false, false, false, null);
			channel.QueueDeclare(PaymentOrderUpdateQueueName, false, false, false, null);

			channel.QueueBind(PaymentEmailUpdateQueueName, ExchangeName, "payment_email");
			channel.QueueBind(PaymentOrderUpdateQueueName, ExchangeName, "payment_order");

			byte[] body = GetMessageAsByteArray(baseMessage);

			channel.BasicPublish(exchange: ExchangeName, "payment_email", basicProperties: null, body: body);
			channel.BasicPublish(exchange: ExchangeName, "payment_order", basicProperties: null, body: body);
		}
	}

	private static byte[] GetMessageAsByteArray(BaseMessage message)
	{
		var options = new JsonSerializerOptions
		{
			WriteIndented = true
		};
		var json = JsonSerializer.Serialize<UpdatePaymentResultMessage>((UpdatePaymentResultMessage)message, options);

		return Encoding.UTF8.GetBytes(json);
	}

	private bool ConnectionExists()
	{
		if (_connection != null)
			return true;

		CreateConnection();
		return _connection != null;
	}

	private void CreateConnection()
	{
		try
		{
			var factory = new ConnectionFactory
			{
				HostName = _hostName,
				UserName = _username,
				Password = _password
			};

			_connection = factory.CreateConnection();
		}
		catch (Exception)
		{
			throw;
		}
	}


}
