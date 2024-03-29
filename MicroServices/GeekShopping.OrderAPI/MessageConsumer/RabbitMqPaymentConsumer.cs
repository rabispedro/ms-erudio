
using System.Text;
using System.Text.Json;
using GeekShopping.OrderAPI.Messages;
using GeekShopping.OrderAPI.Models;
using GeekShopping.OrderAPI.RabbitMqSender.Interfaces;
using GeekShopping.OrderAPI.Repositories;
using GeekShopping.OrderDetailAPI.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace GeekShopping.OrderAPI.MessageConsumer;

public class RabbitMqPaymentConsumer : BackgroundService
{
	private readonly OrderRepository _orderRepository;
	private IConnection _connection;
	private IModel _channel;
	// private const string ExchangeName = "fanout_payment_update_exchange";
	private const string ExchangeName = "direct_payment_update_exchange";
	private const string PaymentOrderUpdateQueueName = "payment_order_update_queue";

	public RabbitMqPaymentConsumer(OrderRepository orderRepository)
	{
		_orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));

		var factory = new ConnectionFactory
		{
			HostName = "localhost",
			Password = "guest",
			UserName = "guest"
		};

		_connection = factory.CreateConnection();
		_channel = _connection.CreateModel();

		_channel.ExchangeDeclare(ExchangeName, ExchangeType.Direct);

		_channel.QueueDeclare(PaymentOrderUpdateQueueName, false, false, false, null);
		_channel.QueueBind(PaymentOrderUpdateQueueName, ExchangeName, "payment_order");
	}

	protected override Task ExecuteAsync(CancellationToken stoppingToken)
	{
		stoppingToken.ThrowIfCancellationRequested();

		var consumer = new EventingBasicConsumer(_channel);
		consumer.Received += (channel, evt) =>
		{
			var content = Encoding.UTF8.GetString(evt.Body.ToArray());
			var updatePaymentResult = JsonSerializer.Deserialize<UpdatePaymentResultMessage>(content);
			UpdatePaymentStatus(updatePaymentResult).GetAwaiter().GetResult();
			_channel.BasicAck(evt.DeliveryTag, false);
		};

		_channel.BasicConsume(PaymentOrderUpdateQueueName, false, consumer);
		return Task.CompletedTask;
	}

	private async Task UpdatePaymentStatus(UpdatePaymentResultMessage paymentResultVo)
	{

		try
		{
			await _orderRepository.UpdateOrderPaymentStatus(paymentResultVo.OrderId, paymentResultVo.Status);
		}
		catch (Exception)
		{
			throw;
		}
	}
}
