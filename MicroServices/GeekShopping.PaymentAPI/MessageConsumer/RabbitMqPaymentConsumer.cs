
using System.Text;
using System.Text.Json;
using GeekShopping.PaymentAPI.Messages;
using GeekShopping.PaymentAPI.RabbitMqSender.Interfaces;
using GeekShopping.PaymentProcessor;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace GeekShopping.PaymentAPI.MessageConsumer;

public class RabbitMqPaymentConsumer : BackgroundService
{
	private IConnection _connection;
	private IModel _channel;
	private IRabbitMqMessageSender _rabbitMqMessageSender;
	private readonly IProcessPayment _proccessPayment;


	public RabbitMqPaymentConsumer(IRabbitMqMessageSender rabbitMqMessageSender, IProcessPayment proccessPayment)
	{
		_rabbitMqMessageSender = rabbitMqMessageSender ?? throw new ArgumentNullException(nameof(rabbitMqMessageSender));
		_proccessPayment = proccessPayment ?? throw new ArgumentNullException(nameof(proccessPayment));

		var factory = new ConnectionFactory
		{
			HostName = "localhost",
			Password = "guest",
			UserName = "guest"
		};

		_connection = factory.CreateConnection();
		_channel = _connection.CreateModel();
		_channel.QueueDeclare(queue: "order_payment_proccess_queue", false, false, false, arguments: null);
	}

	protected override Task ExecuteAsync(CancellationToken stoppingToken)
	{
		stoppingToken.ThrowIfCancellationRequested();

		var consumer = new EventingBasicConsumer(_channel);
		consumer.Received += (channel, evt) =>
		{
			var content = Encoding.UTF8.GetString(evt.Body.ToArray());
			var paymentMessage = JsonSerializer.Deserialize<PaymentMessage>(content);
			ProccessPayment(paymentMessage).GetAwaiter().GetResult();
			_channel.BasicAck(evt.DeliveryTag, false);
		};

		_channel.BasicConsume("order_payment_proccess_queue", false, consumer);
		return Task.CompletedTask;
	}

	private async Task ProccessPayment(PaymentMessage paymentMessage)
	{
		var result = _proccessPayment.PaymentProcessor();

		var paymentResultMessage = new UpdatePaymentResultMessage
		{
			Status = result,
			Email = paymentMessage.Email,
			Id = paymentMessage.Id,
			OrderId = paymentMessage.OrderId
		};
		
		try
		{
			_rabbitMqMessageSender.SendMessage(paymentResultMessage, "order_payment_result_queue");
		}
		catch (Exception)
		{
			throw;
		}
	}
}
