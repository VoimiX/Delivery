﻿using MediatR;

namespace DeliveryApp.Core.Application.UseCases.Commands.Courier.MoveToOrder
{
    public class MoveToOrderCommand : IRequest<MoveToOrderResponse>
    {
        public MoveToOrderCommand(Guid courierId)
        {
            CourierId = courierId;
        }

        public Guid CourierId { get; }
    }
}
