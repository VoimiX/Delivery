using Confluent.Kafka;
using DeliveryApp.Core.Domain.OrderAggregate.DomainEvents;
using DeliveryApp.Core.Ports;
using Newtonsoft.Json;
using OrderStatusChanged;

namespace DeliveryApp.Infrastructure.Adapters.Kafka.OrderEvents;

public class OrderEventsProducer : IBusProducer
{
    private readonly ProducerConfig _config;
    private readonly string _topicName = "order.status.changed";

    public OrderEventsProducer(string messageBrokerHost)
    {
        if (string.IsNullOrWhiteSpace(messageBrokerHost)) throw new ArgumentException(nameof(messageBrokerHost));
        _config = new ProducerConfig
        {
            BootstrapServers = messageBrokerHost
        };
    }

    public async Task PublishOrderAssigned(OrderAssignedDomainEvent notification)
    {
        // Перекладываем данные из Domain Event в Integration Event
        var basketConfirmedIntegrationEvent = new OrderStatusChangedIntegrationEvent
        {
             OrderId = notification.OrderId.ToString(),
             OrderStatus = OrderStatus.Assigned
        };

        // Создаем сообщение для Kafka
        var message = new Message<string, string>
        {
            Key = notification.EventId.ToString(),
            Value = JsonConvert.SerializeObject(basketConfirmedIntegrationEvent)
        };

        // Отправляем сообщение в Kafka
        await SendToKafka(message);
    }

    public async Task PublishOrderCompleted(OrderCompletedDomainEvent notification)
    {
        // Перекладываем данные из Domain Event в Integration Event
        var basketConfirmedIntegrationEvent = new OrderStatusChangedIntegrationEvent
        {
            OrderId = notification.OrderId.ToString(),
            OrderStatus = OrderStatus.Completed
        };

        // Создаем сообщение для Kafka
        var message = new Message<string, string>
        {
            Key = notification.EventId.ToString(),
            Value = JsonConvert.SerializeObject(basketConfirmedIntegrationEvent)
        };

        // Отправляем сообщение в Kafka
        await SendToKafka(message);
    }

    public async Task PublishOrderCreated(OrderCreatedDomainEvent notification)
    {
        // Перекладываем данные из Domain Event в Integration Event
        var basketConfirmedIntegrationEvent = new OrderStatusChangedIntegrationEvent
        {
            OrderId = notification.OrderId.ToString(),
            OrderStatus = OrderStatus.Created
        };

        // Создаем сообщение для Kafka
        var message = new Message<string, string>
        {
            Key = notification.EventId.ToString(),
            Value = JsonConvert.SerializeObject(basketConfirmedIntegrationEvent)
        };

        // Отправляем сообщение в Kafka
        await SendToKafka(message);
    }

    private async Task SendToKafka(Message<string, string> message)
    {
        using var producer = new ProducerBuilder<string, string>(_config).Build();
        await producer.ProduceAsync(_topicName, message, CancellationToken.None);
    }
}
