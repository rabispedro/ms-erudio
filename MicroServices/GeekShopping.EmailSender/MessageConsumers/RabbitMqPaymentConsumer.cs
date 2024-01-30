
using System.Text;
using System.Text.Json;
using GeekShopping.EmailSender.Messages;
using GeekShopping.EmailSender.Repositories;
using GeekShopping.EmailSender.Repositories.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace GeekShopping.EmailSender.MessageConsumers;

public class RabbitMqPaymentConsumer : BackgroundService
{
	private readonly EmailRepository _emailRepository;
	private IConnection _connection;
	private IModel _channel;
	// private const string ExchangeName = "fanout_payment_update_exchange";
	private const string ExchangeName = "direct_payment_update_exchange";
	private const string PaymentEmailUpdateQueueName = "payment_email_update_queue";

	public RabbitMqPaymentConsumer(EmailRepository emailRepository)
	{
		_emailRepository = emailRepository ?? throw new ArgumentNullException(nameof(emailRepository));

		var factory = new ConnectionFactory
		{
			HostName = "localhost",
			Password = "guest",
			UserName = "guest"
		};

		_connection = factory.CreateConnection();
		_channel = _connection.CreateModel();

		_channel.ExchangeDeclare(ExchangeName, ExchangeType.Direct);

		_channel.QueueDeclare(PaymentEmailUpdateQueueName, false, false, false, null);
		_channel.QueueBind(PaymentEmailUpdateQueueName, ExchangeName, "payment_email");
	}

	protected override Task ExecuteAsync(CancellationToken stoppingToken)
	{
		stoppingToken.ThrowIfCancellationRequested();

		var consumer = new EventingBasicConsumer(_channel);
		consumer.Received += (channel, evt) =>
		{
			var content = Encoding.UTF8.GetString(evt.Body.ToArray());
			var updatePaymentResult = JsonSerializer.Deserialize<UpdatePaymentResultMessage>(content);
			ProccessLog(updatePaymentResult).GetAwaiter().GetResult();
			_channel.BasicAck(evt.DeliveryTag, false);
		};

		_channel.BasicConsume(PaymentEmailUpdateQueueName, false, consumer);
		return Task.CompletedTask;
	}

	private async Task ProccessLog(UpdatePaymentResultMessage paymentResultVo)
	{

		try
		{
			await _emailRepository.LogEmail(paymentResultVo);
		}
		catch (Exception)
		{
			throw;
		}
	}
}
