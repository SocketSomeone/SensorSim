using System.Reflection;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Refit;
using SensorSim.Actuator.API.Clients;
using SensorSim.Actuator.API.Interfaces;
using SensorSim.Actuator.API.Services;
using SensorSim.Domain.Model;
using SensorSim.Infrastructure.Repositories;

namespace SensorSim.Actuator.API;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers()
            .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            );
        services.AddEndpointsApiExplorer();
        services.AddCors();

        services.AddSwaggerGen(options =>
        {
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "SensorSim.Actuator.API", Version = "v1" });
        });

        services.AddSingleton<CrudMemoryRepository<ActuatorConfig>, ActuatorConfigsRepository>();
        services.AddSingleton<CrudMemoryRepository<PhysicalQuantity>, QuantitiesRepository>();
        services.AddSingleton<IActuatorService, ActuatorService>();

        services.AddHostedService<ConsumeActuatorHostedService>();

        services.AddRefitClient<ISensorApi>()
            // Get the base address from env
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(Environment.GetEnvironmentVariable("SENSOR_API_BASE_URL") ?? "http://localhost:7000"));
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseCors(x => x
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
        );
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}