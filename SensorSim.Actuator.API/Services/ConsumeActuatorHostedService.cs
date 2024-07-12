using SensorSim.Domain;
using SensorSim.Domain.Interface;

namespace SensorSim.Actuator.API.Services;

public class ConsumeActuatorHostedService : BackgroundService
{
    private readonly ILogger<ConsumeActuatorHostedService> _logger;

    public ConsumeActuatorHostedService(IServiceProvider services,
        ILogger<ConsumeActuatorHostedService> logger)
    {
        Services = services;
        _logger = logger;
    }

    public IServiceProvider Services { get; }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation(
            "Consume Scoped Service Hosted Service running.");

        await DoWork(stoppingToken);
    }

    private async Task DoWork(CancellationToken stoppingToken)
    {
        _logger.LogInformation(
            "Consume Scoped Service Hosted Service is working.");

        // need get all actuators by IActutator<T> interface
        var actuatorService = Services.GetRequiredService<IActuatorService>();

        await actuatorService.Update(stoppingToken);
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation(
            "Consume Scoped Service Hosted Service is stopping.");

        await base.StopAsync(stoppingToken);
    }
}