using System.Reflection;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using SensorSim.Domain.Model;
using SensorSim.Infrastructure.Repositories;
using SensorSim.Sensor.API.Services;

namespace SensorSim.Sensor.API;

public class Startup(IConfiguration configuration)
{
    private readonly IConfiguration _configuration = configuration;

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
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "SensorSim.Sensor.API", Version = "v1" });
        });


        services.AddSingleton<CrudMemoryRepository<PhysicalQuantity>, QuantitiesRepository>();
        services.AddSingleton<CrudMemoryRepository<SensorConfig>, SensorConfigsRepository>();
        services.AddSingleton<ISensorService, SensorService>();
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