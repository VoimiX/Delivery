using DeliveryApp.Core.Application.UseCases.Commands.Courier.AssignOrder;
using DeliveryApp.Core.Application.UseCases.Commands.Courier.MoveToOrder;
using DeliveryApp.Core.Application.UseCases.Commands.Order.CreateOrder;
using DeliveryApp.Core.Application.UseCases.Queries.Courier.GetCouriesReadyBusy;
using DeliveryApp.Core.Application.UseCases.Queries.Order.GetOrdersAssigned;
using DeliveryApp.Core.DomainServices;
using DeliveryApp.Core.Ports;
using DeliveryApp.Infrastructure.Adapters.Postgres;
using DeliveryApp.Infrastructure.Adapters.Postgres.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Primitives;

namespace DeliveryApp.Api
{
    public class Startup
    {
        public Startup()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddEnvironmentVariables();
            var configuration = builder.Build();
            Configuration = configuration;
        }

        /// <summary>
        /// Конфигурация
        /// </summary>
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Health Checks
            services.AddHealthChecks();

            // Cors
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    policy =>
                    {
                        policy.AllowAnyOrigin(); // Не делайте так в проде!
                    });
            });
            
            // Configuration
            services.Configure<Settings>(options => Configuration.Bind(options));
            var connectionString = Configuration["CONNECTION_STRING"];
            var geoServiceGrpcHost = Configuration["GEO_SERVICE_GRPC_HOST"];
            var messageBrokerHost = Configuration["MESSAGE_BROKER_HOST"];

            // UnitOfWork
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            // Ports & Adapters
            services.AddTransient<IOrderRepository, OrderRepository>();
            services.AddTransient<ICourierRepository, CourierRepository>();

            // Domain Services
            services.AddTransient<IDispatchService, DispatchService>();

            // MediatR 
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Startup>());

            // Commands
            services.AddTransient<IRequestHandler<CreateOrderCommand, CreateOrderResponse>, CreateOrderCommandHandler>();
            services.AddTransient<IRequestHandler<MoveToOrderCommand, MoveToOrderResponse>, MoveToOrderCommandHandler>();
            services.AddTransient<IRequestHandler<AssignOrderCommand, AssignOrderResponse>, AssignOrderCommandHandler>();
           
            // Queries
            services.AddTransient<IRequestHandler<GetCouriesReadyBusyQuery, GetCouriesReadyBusyResponse>>(_ =>
                new GetCouriesReadyBusyHandler(connectionString));
            services.AddTransient<IRequestHandler<GetGetOrdersAssignedQuery, GetOrdersAssignedResponse>>(_ =>
                new GetOrdersAssignedHandler(connectionString));



            services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHealthChecks("/health");
            app.UseRouting();
        }
    }
}