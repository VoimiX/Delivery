namespace DeliveryApp.Core.Application.UseCases.Commands.Orders.CreateOrder;

public class CreateOrderResponse
{
    public CreateOrderResponse(Guid orderId)
    {
        OrderId = orderId;
    }

    public Guid OrderId { get;  }
}
