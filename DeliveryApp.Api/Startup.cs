using Api.Filters;
using Api.Formatters;
using Api.OpenApi;
using DeliveryApp.Api.Adapters.BackgroundJobs;
using DeliveryApp.Api.Adapters.Kafka.BasketConfirmed;
using DeliveryApp.Core.Application.UseCases.Commands.Courier.AssignOrder;
using DeliveryApp.Core.Application.UseCases.Commands.Courier.EndWork;
using DeliveryApp.Core.Application.UseCases.Commands.Courier.MoveToOrder;
using DeliveryApp.Core.Application.UseCases.Commands.Courier.StartWork;
using DeliveryApp.Core.Application.UseCases.Commands.Order.CreateOrder;
using DeliveryApp.Core.Application.UseCases.Queries.Courier.GetCouriesReadyBusy;
using DeliveryApp.Core.Application.UseCases.Queries.Order.GetOrdersAssigned;
using DeliveryApp.Core.DomainServices;
using DeliveryApp.Core.Ports;
using DeliveryApp.Infrastructure.Adapters.Grpc.GeoService;
using DeliveryApp.Infrastructure.Adapters.Kafka.OrderEvents;
using DeliveryApp.Infrastructure.Adapters.Postgres;
using DeliveryApp.Infrastructure.Adapters.Postgres.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Primitives;
using Quartz;
using System.Reflection;
using static Confluent.Kafka.ConfigPropertyNames;

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
            services.AddTransient<IGeoServiceClient>(_ => new GeoServiceClient(geoServiceGrpcHost));
            services.AddTransient<IBusProducer>(_ => new OrderEventsProducer(messageBrokerHost));

            // Domain Services
            services.AddTransient<IDispatchService, DispatchService>();

            // MediatR 
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Startup>());

            // Commands
            services.AddTransient<IRequestHandler<CreateOrderCommand, CreateOrderResponse>, CreateOrderHandler>();
            services.AddTransient<IRequestHandler<MoveToOrderCommand, MoveToOrderResponse>, MoveToOrderHandler>();
            services.AddTransient<IRequestHandler<AssignOrderCommand, AssignOrderResponse>, AssignOrderHandler>();
            services.AddTransient<IRequestHandler<StartWorkCommand, StartWorkResponse>, StartWorkHandler>();
            services.AddTransient<IRequestHandler<EndWorkCommand, EndWorkResponse>, EndWorkHandler>();

            // Queries
            services.AddTransient<IRequestHandler<GetCouriesReadyBusyQuery, GetCouriesReadyBusyResponse>>(_ =>
                new GetCouriesReadyBusyHandler(connectionString));
            services.AddTransient<IRequestHandler<GetGetOrdersAssignedQuery, GetOrdersAssignedResponse>>(_ =>
                new GetOrdersAssignedHandler(connectionString));

            services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));

            // HTTP Handlers
            services.AddControllers(options =>
            {
                options.InputFormatters.Insert(0, new InputFormatterStream());
            })
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.Converters.Add(new StringEnumConverter
                    {
                        NamingStrategy = new CamelCaseNamingStrategy()
                    });
                });

            // Swagger
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("1.0.0", new OpenApiInfo
                {
                    Title = "Delivery Service",
                    Description = "Отвечает за учет курьеров, деспетчеризацию доставкуов, доставку",
                    Contact = new OpenApiContact
                    {
                        Name = "Vitalii Sitnikov",
                        Email = "vs@supermail.com"
                    }
                });
                options.CustomSchemaIds(type => type.FriendlyId(true));
                options.IncludeXmlComments($"{AppContext.BaseDirectory}{Path.DirectorySeparatorChar}{Assembly.GetEntryAssembly().GetName().Name}.xml");
                options.DocumentFilter<BasePathFilter>("");
                options.OperationFilter<GeneratePathParamsValidationFilter>();
            });
            services.AddSwaggerGenNewtonsoftSupport();

            // gRPC
            services.AddGrpcClient<GeoServiceClient>(options => { options.Address = new Uri(geoServiceGrpcHost); });

            // Message Broker
            var sp = services.BuildServiceProvider();
            var mediator = sp.GetService<IMediator>();
            services.AddHostedService(x => new ConsumerService(mediator, messageBrokerHost));

            //CRON Jobs
            services.AddQuartz(configure =>
            {
                var assignOrdersJobKey = new JobKey(nameof(AssignOrdersJob));
                var moveCouriersJobKey = new JobKey(nameof(MoveCouriersJob));
                configure
                    .AddJob<AssignOrdersJob>(assignOrdersJobKey)
                    .AddTrigger(
                        trigger => trigger.ForJob(assignOrdersJobKey)
                            .WithSimpleSchedule(
                                schedule => schedule.WithIntervalInSeconds(5)
                                    .RepeatForever()))
                    .AddJob<MoveCouriersJob>(moveCouriersJobKey)
                    .AddTrigger(
                        trigger => trigger.ForJob(moveCouriersJobKey)
                            .WithSimpleSchedule(
                                schedule => schedule.WithIntervalInSeconds(3)
                                    .RepeatForever()));
                configure.UseMicrosoftDependencyInjectionJobFactory();
            });
            services.AddQuartzHostedService();
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

            app.UseRouting();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "openapi/{documentName}/openapi.json";
            })
                .UseSwaggerUI(options =>
                {
                    options.RoutePrefix = "openapi";
                    options.SwaggerEndpoint("/openapi/1.0.0/openapi.json", "Swagger Delivery Service");
                    options.RoutePrefix = string.Empty;
                    options.SwaggerEndpoint("/openapi-original.json", "Swagger Delivery Service");
                });

            app.UseCors();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseHealthChecks("/health");
            app.UseRouting();
        }
    }
}