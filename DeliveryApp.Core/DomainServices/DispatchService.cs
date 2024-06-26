﻿using DeliveryApp.Core.Domain.CourierAggregate;
using DeliveryApp.Core.Domain.Exceptions;
using DeliveryApp.Core.Domain.OrderAggregate;

namespace DeliveryApp.Core.DomainServices;

public class DispatchService : IDispatchService
{
    public Task<Courier> Dispatch(Order order, IReadOnlyList<Courier> couriers)
    {
        if (couriers.Any(c => c.Status != CourierStatus.Ready))
        {
            throw new DeliveryException("В списке курьеров есть как минимум один неактивный");
        }

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

        if (bestCourier == null) return Task.FromResult((Courier)null);

        bestCourier.AssignOrder(order);

        return Task.FromResult(bestCourier);
    }
}
