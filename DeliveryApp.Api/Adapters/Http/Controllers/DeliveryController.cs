﻿using Api.Controllers;
using Api.Models;
using DeliveryApp.Core.Application.UseCases.Commands.Courier.EndWork;
using DeliveryApp.Core.Application.UseCases.Commands.Courier.StartWork;
using DeliveryApp.Core.Application.UseCases.Commands.Order.CreateOrder;
using DeliveryApp.Core.Application.UseCases.Queries.Courier.GetCouriesReadyBusy;
using DeliveryApp.Core.Application.UseCases.Queries.Order.GetOrdersAssigned;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DeliveryApp.Api.Adapters.Http.Controllers;

public class DeliveryController : DefaultApiController
{
    private readonly IMediator _mediator;

    public DeliveryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async Task<IActionResult> CreateOrder()
    {
        var rnd = Random.Shared;

        var response = await _mediator.Send(new CreateOrderCommand(
            Guid.NewGuid(),
            address: "Айтишная",
            weight: rnd.Next(1, 8)
            ));

        return Ok();
    }

    public async override Task<IActionResult> GetCouriers()
    {
        var response = await _mediator.Send(new GetCouriesReadyBusyQuery());

        var apiCouries = response.Couriers.Select(c => new Courier
        {
            Id = c.Id,
            Location = new Location { X = c.Location.X, Y = c.Location.Y },
            Name = c.Name,
        });

        return Ok(apiCouries);
    }

    public async override Task<IActionResult> GetOrders()
    {
        var response = await _mediator.Send(new GetGetOrdersAssignedQuery());

        var apiOrders = response.Orders.Select(o => new Order
        {
            Id = o.Id,
            Location = new Location { X = o.Location.X, Y = o.Location.Y }
        });

        return Ok(apiOrders);
    }

    public override async Task<IActionResult> StartWork([FromRoute(Name = "courierId"), Required] Guid courierId)
    {
        var response = await _mediator.Send(new StartWorkCommand(courierId));

        return Ok();
    }

    public async override Task<IActionResult> StopWork([FromRoute(Name = "courierId"), Required] Guid courierId)
    {
        var response = await _mediator.Send(new EndWorkCommand(courierId));

        return Ok();
    }
}
