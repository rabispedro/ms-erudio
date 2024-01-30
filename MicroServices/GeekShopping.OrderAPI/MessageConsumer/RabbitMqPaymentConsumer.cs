
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
		_channel.QueueDeclare(queue: "order_payment_result_queue", false, false, false, arguments: null);
	}

	protected override Task ExecuteAsync(CancellationToken stoppingToken)
	{
		stoppingToken.ThrowIfCancellationRequested();

		var consumer = new EventingBasicConsumer(_channel);
		consumer.Received += (channel, evt) =>
		{
			var content = Encoding.UTF8.GetString(evt.Body.ToArray());
			var updatePaymentResult = JsonSerializer.Deserialize<UpdatePaymentResultVO>(content);
			UpdatePaymentStatus(updatePaymentResult).GetAwaiter().GetResult();
			_channel.BasicAck(evt.DeliveryTag, false);
		};

		_channel.BasicConsume("order_payment_result_queue", false, consumer);
		return Task.CompletedTask;
	}

	private async Task UpdatePaymentStatus(UpdatePaymentResultVO paymentResultVo)
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
