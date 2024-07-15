using SensorSim.Actuator.API.Clients;
using SensorSim.Actuator.API.Interfaces;
using SensorSim.Domain.Model;
using SensorSim.Infrastructure.Helpers;
using SensorSim.Infrastructure.Repositories;

namespace SensorSim.Actuator.API.Services;

public class ActuatorService(
    ILogger<ActuatorService> logger,
    CrudMemoryRepository<ActuatorConfig> actuatorConfigsRepository,
    CrudMemoryRepository<PhysicalQuantity> quantitiesRepository,
    CrudMemoryRepository<ActuatorEvent> actuatorEventsRepository,
    ISensorApi sensorApi) : IActuatorService
{
    public event IActuatorService.ValueChangedEventHandler? ValueChangedEvent;

    public event IActuatorService.ValueReachedExposureEventHandler? ValueReachedExposureEvent;

    private ILogger<ActuatorService> Logger { get; } = logger;

    private CrudMemoryRepository<ActuatorConfig> ActuatorConfigsRepository { get; } = actuatorConfigsRepository;

    private CrudMemoryRepository<PhysicalQuantity> QuantitiesRepository { get; } = quantitiesRepository;

    private CrudMemoryRepository<ActuatorEvent> ActuatorEventsRepository { get; } = actuatorEventsRepository;

    private ISensorApi SensorApi { get; } = sensorApi;

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

    public IEnumerable<ActuatorEvent> GetEvents(string id)
    {
        return ActuatorEventsRepository.GetAll().Where(e => e.ActuatorId == id).OrderByDescending(e => e.CreatedAt);
    }

    public async Task Update(CancellationToken stoppingToken)
    {
        const int timeUpdate = 250;

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

                if (exposures.Count <= 0 || waiting) continue;
                var exposure = exposures.Peek();
                var measurement = QuantitiesRepository.GetOrDefault(actuatorId);

                if (measurement.Value.Equals(exposure.Value))
                {
                    exposures.Dequeue();
                    config.WaitUntil = DateTime.Now.AddSeconds(exposure.Duration);
                    ValueReachedExposureEvent?.Invoke(this, actuatorId, exposure);
                    
                    var eventType = "ValueReachedExposure";
                    
                    if (config.Exposures.Count == 0)
                    {
                        eventType = "ValueReachedTarget";
                    }
                    
                    actuatorEventsRepository.Add(new ActuatorEvent($"{actuatorId}:{DateTime.Now}:{DateTime.Now.Millisecond}:{eventType}")
                    {
                        ActuatorId = actuatorId,
                        Name = eventType,
                        Value = measurement.Value
                    });
                }
                else
                {
                    var motion = new InertiaMotionFunction(timeUpdate / 1000.0);
                    var value = motion.Calculate(measurement.Value, exposure.Value, exposure.Speed);
                    SetCurrentQuantity(actuatorId, value, measurement.Unit);
                    await SensorApi.SetQuantity(actuatorId, new() { Value = measurement.Value, Unit = measurement.Unit });
                    
                    actuatorEventsRepository.Add(new ActuatorEvent($"{actuatorId}:{DateTime.Now}:{DateTime.Now.Millisecond}:ValueChanged")
                    {
                        ActuatorId = actuatorId,
                        Name = "ValueChanged",
                        Value = measurement.Value
                    });
                    ValueChangedEvent?.Invoke(this, actuatorId);
                }
            }

            await Task.Delay(timeUpdate, stoppingToken);
        }
    }
}