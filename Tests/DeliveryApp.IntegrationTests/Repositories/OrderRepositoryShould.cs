using DeliveryApp.Core.Domain.SharedKernel;
using DeliveryApp.Infrastructure.Adapters.Postgres;
using DeliveryApp.Infrastructure.Adapters.Postgres.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;
using Xunit;

namespace DeliveryApp.IntegrationTests.Repositories
{
    public class OrderRepositoryShould : IAsyncLifetime
    {
        private ApplicationDbContext _context;

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
        public OrderRepositoryShould()
        {
        }

        [Fact]
        public async Task Can_Get_Unassigned_Orders()
        {
            var orderRepository = new OrderRepository(_context);

            await orderRepository.AddOrder(new Core.Domain.OrderAggregate.Order(
                 Guid.NewGuid(), new Location(2, 8), new Weight(5))
                );

            await orderRepository.AddOrder(new Core.Domain.OrderAggregate.Order(
                 Guid.NewGuid(), new Location(4, 5), new Weight(8))
                );

            await _context.SaveChangesAsync();

            var ordersNew = await orderRepository.GetOrdersNew();
            ordersNew.Should().NotBeNull();
            ordersNew.Count().Should().Be(2);
        }

        public async Task InitializeAsync()
        {
            //Стартуем БД (библиотека TestContainers запускает Docker контейнер с Postgres)
            await _postgreSqlContainer  .StartAsync();

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
}
