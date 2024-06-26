﻿using BasketConfirmed;
using Confluent.Kafka;
using DeliveryApp.Core.Application.UseCases.Commands.Order.CreateOrder;
using MediatR;
using Newtonsoft.Json;

namespace DeliveryApp.Api.Adapters.Kafka.BasketConfirmed;

public class ConsumerService : BackgroundService
{
    private readonly IMediator _mediator;
    private readonly IConsumer<Ignore, string> _consumer;

    public ConsumerService(IMediator mediator, string messageBrokerHost)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        if (string.IsNullOrWhiteSpace(messageBrokerHost)) throw new ArgumentException(nameof(messageBrokerHost));

        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = messageBrokerHost,
            GroupId = "DeliveryConsumerGroup",
            EnableAutoOffsetStore = false,
            EnableAutoCommit = true,
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnablePartitionEof = true
        };
        _consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        _consumer.Subscribe("basket.confirmed");
        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
                var consumeResult = _consumer.Consume(cancellationToken);

                if (consumeResult.IsPartitionEOF)
                {
                    continue;
                }

                Console.WriteLine($"Received message at {consumeResult.TopicPartitionOffset}: {consumeResult.Message.Value}");
                
                var basketConfirmedIntegrationEvent = JsonConvert.DeserializeObject<BasketConfirmedIntegrationEvent>
                    (consumeResult.Message.Value);

                //Тут ваш Use Case
                await _mediator.Send(new CreateOrderCommand(
                    orderId: Guid.Parse(basketConfirmedIntegrationEvent.BasketId), 
                    address: basketConfirmedIntegrationEvent.Address,
                    basketConfirmedIntegrationEvent.Weight
                    ));

                try
                {
                    _consumer.StoreOffset(consumeResult);
                }
                catch (KafkaException e)
                {
                    Console.WriteLine($"Store Offset error: {e.Error.Reason}");
                }
            }
        }
        catch (OperationCanceledException)
        {
            _consumer.Close();
        }
    }
}
