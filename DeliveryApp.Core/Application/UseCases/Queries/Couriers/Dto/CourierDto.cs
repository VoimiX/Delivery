﻿
namespace DeliveryApp.Core.Application.UseCases.Queries.Couriers.Dto;

public class CourierDto
{
    public CourierDto(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public int Id { get; }
    public string Name { get; }
}
