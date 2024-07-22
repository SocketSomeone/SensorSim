using SensorSim.Actuator.API.Interface;

namespace SensorSim.Actuator.API.Services;

public class ConsumeActuatorHostedService(
    IServiceProvider services,
    ILogger<ConsumeActuatorHostedService> logger)
    : BackgroundService
{
    public IServiceProvider Services { get; } = services;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation(
            "Consume Scoped Service Hosted Service running.");

        await DoWork(stoppingToken);
    }

    private async Task DoWork(CancellationToken stoppingToken)
    {
        logger.LogInformation(
            "Consume Scoped Service Hosted Service is working.");

        var actuatorService = Services.GetRequiredService<IActuatorService>();

        await actuatorService.Update(stoppingToken);
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation(
            "Consume Scoped Service Hosted Service is stopping.");

        await base.StopAsync(stoppingToken);
    }
}