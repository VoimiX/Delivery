using DeliveryApp.Core.Domain.CourierAggregate;
using DeliveryApp.Infrastructure.Adapters.Postgres;
using DeliveryApp.Infrastructure.Adapters.Postgres.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;
using Xunit;

namespace DeliveryApp.Integration.Repositories;

public class CourierRepositoryShould : IAsyncLifetime
{
    private ApplicationDbContext _context;
    private readonly Courier _courier;

    /// <summary>
    /// Настройка Postgres из библиотеки TestContainers
    /// </summary>
    /// <remarks>По сути это Docker контейнер с Postgres</remarks>
    private readonly PostgreSqlContainer _postgreSqlContainer = new PostgreSqlBuilder()
        .WithImage("postgres:13.4")
        .WithDatabase("delivery")
        .WithUsername("username")
        .WithPassword("secret")
        .WithCleanUp(true)
        .Build();

    /// <remarks>Вызывается один раз перед всеми тестами в рамках этого класса</remarks>
    public CourierRepositoryShould()
    {
        //var courierCreateResult = new Courier(new Guid(""), "Gazel", new Car());

    }

    [Fact]
    public async Task CanSaveCourier()
    {
        var transportCar = Transport.All.Single(t => t.Name == typeof(Car).Name);
        Courier courier = new Courier(id: Guid.NewGuid(), name: "Василий", transportCar);        

        var courierRepository = new CourierRepository(_context);
        await courierRepository.AddCourier(courier);

        var unitOfWork = new UnitOfWork(_context);
        await unitOfWork.SaveEntitiesAsync();

        var courierFromdb = await courierRepository.GetCourier(courier.Id);
        courierFromdb.Should().BeEquivalentTo(courier);
    }


    public async Task InitializeAsync()
    {
        //Стартуем БД (библиотека TestContainers запускает Docker контейнер с Postgres)
        await _postgreSqlContainer.StartAsync();

        //Накатываем миграции и справочники
        var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>().UseNpgsql(
            _postgreSqlContainer.GetConnectionString(),
            npgsqlOptionsAction: sqlOptions => { sqlOptions.MigrationsAssembly("DeliveryApp.Infrastructure"); }).Options;
        _context = new ApplicationDbContext(contextOptions);
        _context.Database.Migrate();
    }

    public async Task DisposeAsync()
    {
        await _postgreSqlContainer.DisposeAsync().AsTask();
    }
}

