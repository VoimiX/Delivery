﻿using DeliveryApp.Core.Domain.CourierAggregate;
using DeliveryApp.Core.Domain.OrderAggregate;

namespace DeliveryApp.Core.DomainServices;

public class DispatchService : IDispatchService
{
    public Task<Courier> Dispatch(Order order, IReadOnlyList<Courier> couriers)
    {
        if (couriers.Count == 0)  throw new ArgumentException("Пустой список курьеров" , nameof(couriers)); 

        Courier bestCourier = null;
        float? minSteps = null;

        foreach (var courier in couriers)
        {
            if (!courier.Transport.CanHandleWeight(order.Weight)) continue;

            var steps = courier.GetStepsDistanceToOrder(order.Location);
            if (minSteps == null || steps <= minSteps)
            {
                bestCourier = courier;
                minSteps = steps;
            }
        }

        return Task.FromResult(bestCourier);
    }
}