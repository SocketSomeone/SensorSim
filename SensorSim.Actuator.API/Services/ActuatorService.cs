using SensorSim.Actuator.API.Clients;
using SensorSim.Domain;
using SensorSim.Domain.DTO.Actuator;
using SensorSim.Domain.DTO.Sensor;
using SensorSim.Domain.Interface;
using SensorSim.Domain.Model;
using SensorSim.Infrastructure.Helpers;
using SensorSim.Infrastructure.Repositories;

namespace SensorSim.Actuator.API.Services;

public class ActuatorEvent
{
    public string Name { get; set; }

    public object Value { get; set; }
}

public interface IActuatorService
{
    public delegate void ValueChangedEventHandler(object sender, string actuatorId);

    public event ValueChangedEventHandler ValueChangedEvent;

    public delegate void ValueReachedEventHandler(object sender, string actuatorId, PhysicalExposure exposure);

    public event ValueReachedEventHandler ValueReachedEvent;

    string[] GetActuators();

    PhysicalQuantity ReadCurrentQuantity(string id);

    PhysicalQuantity ReadTargetQuantity(string id);

    Queue<PhysicalExposure> ReadExposures(string id);

    Task Update(CancellationToken stoppingToken);
    
    void SetCurrentQuantity(string actuatorId, double value, string unit);
    void SetTargetQuantity(string actuatorId, double value, string unit);
    
    void SetExposures(string actuatorId, Queue<PhysicalExposure> exposures);
}

public class ActuatorService(
    ILogger<ActuatorService> logger,
    CrudMemoryRepository<ActuatorConfig> actuatorConfigsRepository,
    CrudMemoryRepository<PhysicalQuantity> quantitiesRepository,
    ISensorApi sensorApi) : IActuatorService
{
    public event IActuatorService.ValueChangedEventHandler? ValueChangedEvent;

    public event IActuatorService.ValueReachedEventHandler? ValueReachedEvent;

    private ILogger<ActuatorService> Logger { get; set; } = logger;

    private CrudMemoryRepository<ActuatorConfig> ActuatorConfigsRepository { get; set; } = actuatorConfigsRepository;

    private CrudMemoryRepository<PhysicalQuantity> QuantitiesRepository { get; set; } = quantitiesRepository;

    private ISensorApi SensorApi { get; set; } = sensorApi;

    public string[] GetActuators()
    {
        return ActuatorConfigsRepository.GetAll().Select(q => q.Id).ToArray();
    }
    
    public void SetCurrentQuantity(string actuatorId, double value, string unit)
    {
        var quantity = QuantitiesRepository.GetOrDefault(actuatorId);
        quantity.Value = value;
        quantity.Unit = unit;
    }

    public void SetTargetQuantity(string actuatorId, double value, string unit)
    {
        var config = ActuatorConfigsRepository.GetOrDefault(actuatorId);
        config.TargetQuantity.Value = value;
        config.TargetQuantity.Unit = unit;
    }
    
    public void SetExposures(string actuatorId, Queue<PhysicalExposure> exposures)
    {
        var config = ActuatorConfigsRepository.GetOrDefault(actuatorId);
        config.Exposures = exposures;
    }

    public PhysicalQuantity ReadCurrentQuantity(string id)
    {
        return QuantitiesRepository.GetOrDefault(id);
    }

    public PhysicalQuantity ReadTargetQuantity(string id)
    {
        return ActuatorConfigsRepository.GetOrDefault(id).TargetQuantity;
    }

    public Queue<PhysicalExposure> ReadExposures(string id)
    {
        return ActuatorConfigsRepository.GetOrDefault(id).Exposures;
    }

    public async Task Update(CancellationToken stoppingToken)
    {
        const int timeUpdate = 250;
        
        ValueChangedEvent += async (sender, actuatorId) =>
        {
            var currentQuantity = QuantitiesRepository.GetOrDefault(actuatorId);

            await SensorApi.SetQuantity(actuatorId, new(currentQuantity.Value, currentQuantity.Unit));

            var sensorValue = await SensorApi.ReadQuantity(actuatorId);
            Logger.LogInformation("ValueChanged: {CurrentQuantityValue} | {SensorValueParameter}",
                currentQuantity.Value, sensorValue.Parameter);
        };

        ValueReachedEvent += async (sender, actuatorId, exposure) =>
        {
            var currentQuantity = QuantitiesRepository.GetOrDefault(actuatorId);
            var config = ActuatorConfigsRepository.GetOrDefault(actuatorId);
            var sensorValue = await SensorApi.ReadQuantity(actuatorId);

            Logger.LogInformation("ValueReached: {CurrentQuantityValue} | {SensorValueParameter}",
                currentQuantity.Value, sensorValue.Parameter);
            config.WaitUntil = DateTime.Now.AddSeconds(exposure.Duration);
            Logger.LogInformation("Waiting until: {ConfigWaitUntil}", config.WaitUntil);
        };

        while (!stoppingToken.IsCancellationRequested)
        {
            var actuatorIds = GetActuators();

            foreach (var actuatorId in actuatorIds)
            {
                var config = ActuatorConfigsRepository.GetOrDefault(actuatorId);

                var exposures = config.Exposures;

                var waiting = DateTime.Now < config.WaitUntil;

                if (waiting)
                {
                    Logger.LogInformation("Waiting... and waiting... and waiting...");
                    continue;
                }

                if (exposures.Count > 0 && !waiting)
                {
                    var exposure = exposures.Peek();
                    var measurement = QuantitiesRepository.GetOrDefault(actuatorId);

                    if (measurement.Value.Equals(exposure.Value))
                    {
                        exposures.Dequeue();
                        ValueReachedEvent?.Invoke(this, actuatorId, exposure);
                    }
                    else
                    {
                        var motion = new InertiaMotionFunction(timeUpdate / 1000.0);
                        var value = motion.Calculate(measurement.Value, exposure.Value, exposure.Speed);
                        SetCurrentQuantity(actuatorId, value, measurement.Unit);
                        ValueChangedEvent?.Invoke(this, actuatorId);
                    }
                }
            }

            await Task.Delay(timeUpdate, stoppingToken);
        }
    }
}