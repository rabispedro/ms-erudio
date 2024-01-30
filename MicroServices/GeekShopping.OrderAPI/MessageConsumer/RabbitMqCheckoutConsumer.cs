
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

public class RabbitMqCheckoutConsumer : BackgroundService
{
	private readonly OrderRepository _orderRepository;
	private IConnection _connection;
	private IModel _channel;
	private IRabbitMqMessageSender _rabbitMqMessageSender;

	public RabbitMqCheckoutConsumer(OrderRepository orderRepository, IRabbitMqMessageSender rabbitMqMessageSender)
	{
		_orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
		_rabbitMqMessageSender = rabbitMqMessageSender ?? throw new ArgumentNullException(nameof(rabbitMqMessageSender));

		var factory = new ConnectionFactory
		{
			HostName = "localhost",
			Password = "guest",
			UserName = "guest"
		};

		_connection = factory.CreateConnection();
		_channel = _connection.CreateModel();
		_channel.QueueDeclare(queue: "checkout_queue", false, false, false, arguments: null);
	}

	protected override Task ExecuteAsync(CancellationToken stoppingToken)
	{
		stoppingToken.ThrowIfCancellationRequested();

		var consumer = new EventingBasicConsumer(_channel);
		consumer.Received += (channel, evt) =>
		{
			var content = Encoding.UTF8.GetString(evt.Body.ToArray());
			var checkoutHeaderVo = JsonSerializer.Deserialize<CheckoutHeaderVO>(content);
			ProccessOrder(checkoutHeaderVo).GetAwaiter().GetResult();
			_channel.BasicAck(evt.DeliveryTag, false);
		};

		_channel.BasicConsume("checkout_queue", false, consumer);
		return Task.CompletedTask;
	}

	private async Task ProccessOrder(CheckoutHeaderVO checkoutHeaderVo)
	{
		OrderHeader orderHeader = new()
		{
			UserId = checkoutHeaderVo.UserId,
			FirstName = checkoutHeaderVo.FirstName,
			LastName = checkoutHeaderVo.LastName,
			OrderDetails = new List<OrderDetail>(),
			CardNumber = checkoutHeaderVo.CardNumber,
			CouponCode = checkoutHeaderVo.CouponCode,
			CVV = checkoutHeaderVo.CVV,
			DateTime = checkoutHeaderVo.DateTime,
			DiscountAmount = checkoutHeaderVo.DiscountAmount,
			Email = checkoutHeaderVo.Email,
			ExpiryMonthYear = checkoutHeaderVo.ExpiryMonthYear,
			Id = checkoutHeaderVo.Id,
			OrderTime = DateTime.UtcNow,
			PaymentStatus = false,
			Phone = checkoutHeaderVo.Phone,
			PurchaseAmount = checkoutHeaderVo.PurchaseAmount
		};

		foreach (var detail in checkoutHeaderVo.CartDetails)
		{
			var item = new OrderDetail
			{
				ProductId = detail.ProductId,
				ProductName = detail.Product.Name,
				Price = detail.Product.Price,
				Count = detail.Count,
			};

			orderHeader.TotalItems += detail.Count;
			orderHeader.OrderDetails.Add(item);
		}

		await _orderRepository.AddOrder(orderHeader);

		var payment = new PaymentVO
		{
			Name = orderHeader.FirstName + " " + orderHeader.LastName,
			CardNumber = orderHeader.CardNumber,
			CVV = orderHeader.CVV,
			Email = orderHeader.Email,
			ExpiryMonthYear = orderHeader.ExpiryMonthYear,
			OrderId = orderHeader.Id,
			PurchaseAmount = orderHeader.PurchaseAmount
		};

		try
		{
			_rabbitMqMessageSender.SendMessage(payment, "order_payment_proccess_queue");
		}
		catch (Exception)
		{
			throw;
		}
	}
}
