﻿// <auto-generated />
using System;
using DeliveryApp.Infrastructure.Adapters.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DeliveryApp.Infrastructure.Adapters.Postgres.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240609142515_ChangeStatusName")]
    partial class ChangeStatusName
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("DeliveryApp.Core.Domain.CourierAggregate.Courier", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<int>("Status")
                        .HasColumnType("integer")
                        .HasColumnName("status");

                    b.Property<int>("transport_id")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("transport_id");

                    b.ToTable("couriers", (string)null);
                });

            modelBuilder.Entity("DeliveryApp.Core.Domain.CourierAggregate.Transport", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    b.Property<int>("Capacity")
                        .HasColumnType("integer")
                        .HasColumnName("capacity");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(13)
                        .HasColumnType("character varying(13)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<int>("Speed")
                        .HasColumnType("integer")
                        .HasColumnName("speed");

                    b.HasKey("Id");

                    b.ToTable("transports", (string)null);

                    b.HasDiscriminator<string>("Discriminator").HasValue("Transport");

                    b.UseTphMappingStrategy();

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Capacity = 4,
                            Name = "Bicycle",
                            Speed = 2
                        },
                        new
                        {
                            Id = 2,
                            Capacity = 8,
                            Name = "Car",
                            Speed = 4
                        },
                        new
                        {
                            Id = 3,
                            Capacity = 1,
                            Name = "Pedestrian",
                            Speed = 1
                        },
                        new
                        {
                            Id = 4,
                            Capacity = 6,
                            Name = "Scooter",
                            Speed = 3
                        });
                });

            modelBuilder.Entity("DeliveryApp.Core.Domain.OrderAggregate.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid?>("CourierId")
                        .HasColumnType("uuid")
                        .HasColumnName("courier_id");

                    b.Property<int>("Status")
                        .HasColumnType("integer")
                        .HasColumnName("status");

                    b.HasKey("Id");

                    b.ToTable("orders", (string)null);
                });

            modelBuilder.Entity("DeliveryApp.Core.Domain.CourierAggregate.Bicycle", b =>
                {
                    b.HasBaseType("DeliveryApp.Core.Domain.CourierAggregate.Transport");

                    b.HasDiscriminator().HasValue("Bicycle");
                });

            modelBuilder.Entity("DeliveryApp.Core.Domain.CourierAggregate.Car", b =>
                {
                    b.HasBaseType("DeliveryApp.Core.Domain.CourierAggregate.Transport");

                    b.HasDiscriminator().HasValue("Car");
                });

            modelBuilder.Entity("DeliveryApp.Core.Domain.CourierAggregate.Pedestrian", b =>
                {
                    b.HasBaseType("DeliveryApp.Core.Domain.CourierAggregate.Transport");

                    b.HasDiscriminator().HasValue("Pedestrian");
                });

            modelBuilder.Entity("DeliveryApp.Core.Domain.CourierAggregate.Scooter", b =>
                {
                    b.HasBaseType("DeliveryApp.Core.Domain.CourierAggregate.Transport");

                    b.HasDiscriminator().HasValue("Scooter");
                });

            modelBuilder.Entity("DeliveryApp.Core.Domain.CourierAggregate.Courier", b =>
                {
                    b.HasOne("DeliveryApp.Core.Domain.CourierAggregate.Transport", "Transport")
                        .WithMany()
                        .HasForeignKey("transport_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("DeliveryApp.Core.Domain.SharedKernel.Location", "Location", b1 =>
                        {
                            b1.Property<Guid>("CourierId")
                                .HasColumnType("uuid");

                            b1.Property<int>("X")
                                .HasColumnType("integer")
                                .HasColumnName("location_x");

                            b1.Property<int>("Y")
                                .HasColumnType("integer")
                                .HasColumnName("location_y");

                            b1.HasKey("CourierId");

                            b1.ToTable("couriers");

                            b1.WithOwner()
                                .HasForeignKey("CourierId");
                        });

                    b.Navigation("Location");

                    b.Navigation("Transport");
                });

            modelBuilder.Entity("DeliveryApp.Core.Domain.OrderAggregate.Order", b =>
                {
                    b.OwnsOne("DeliveryApp.Core.Domain.SharedKernel.Location", "Location", b1 =>
                        {
                            b1.Property<Guid>("OrderId")
                                .HasColumnType("uuid");

                            b1.Property<int>("X")
                                .HasColumnType("integer")
                                .HasColumnName("location_x");

                            b1.Property<int>("Y")
                                .HasColumnType("integer")
                                .HasColumnName("location_y");

                            b1.HasKey("OrderId");

                            b1.ToTable("orders");

                            b1.WithOwner()
                                .HasForeignKey("OrderId");
                        });

                    b.OwnsOne("DeliveryApp.Core.Domain.SharedKernel.Weight", "Weight", b1 =>
                        {
                            b1.Property<Guid>("OrderId")
                                .HasColumnType("uuid");

                            b1.Property<int>("Kilograms")
                                .HasColumnType("integer")
                                .HasColumnName("weight");

                            b1.HasKey("OrderId");

                            b1.ToTable("orders");

                            b1.WithOwner()
                                .HasForeignKey("OrderId");
                        });

                    b.Navigation("Location");

                    b.Navigation("Weight");
                });
#pragma warning restore 612, 618
        }
    }
}
