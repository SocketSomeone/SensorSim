using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SensorSim.Domain.DTO.Actuator;
using SensorSim.Domain.Interface;

namespace SensorSim.Domain.Actuator;

public class ActuatorEvent
{
    public string Name { get; set; }
    
    public object Value { get; set; }
}

public class Actuator<T> : IActuator<T> where T : IPhysicalQuantity
{
    public event IActuator<T>.ValueChangedEventHandler? ValueChangedEvent;
    
    public event IActuator<T>.ValueReachedEventHandler? ValueReachedEvent;

    public ILogger<IActuator<T>> Logger { get; set; }

    public IActuatorConfig<T> Config { get; set; }

    public ISensor<T> Sensor { get; set; }

    public T CurrentQuantity { get; set; }

    public T TargetQuantity { get; set; }

    public Queue<PhysicalValueExposure> Exposures { get; set; }

    private Timer _updateTimer;
    
    private DateTime _waitUntil;

    public Actuator(ILogger<Actuator<T>> logger, IActuatorConfig<T> config, ISensor<T> sensor)
    {
        Logger = logger;
        Config = config;
        Sensor = sensor;
        Exposures = new();

        CurrentQuantity = (T)Activator.CreateInstance(typeof(T), Config.InitialQuantity.Value);
        TargetQuantity = (T)Activator.CreateInstance(typeof(T), Config.InitialQuantity.Value);
        _waitUntil = DateTime.Now;

        ValueChangedEvent += (sender, exposure) =>
        {
            Sensor.SetQuantity(CurrentQuantity.Value);
            Logger.LogInformation($"ValueChanged: {CurrentQuantity.Value} | {Sensor.ReadParameter()}");
        };
        
        ValueReachedEvent += (sender, exposure) =>
        {
            Logger.LogInformation($"ValueReached: {CurrentQuantity.Value} | {Sensor.ReadParameter()}");
            _waitUntil = DateTime.Now.AddSeconds(exposure.Duration);
            Logger.LogInformation($"Waiting until: {_waitUntil}");
        };
    }

    public ActuatorResponseModels.SetActuatorResponseModel Set(double target, Queue<PhysicalValueExposure> exposures)
    {
        if (exposures.Count == 0 || !exposures.Last().Value.Equals(target))
        {
            exposures.Enqueue(new PhysicalValueExposure
            {
                Value = target,
                Duration = 1,
                Speed = 1.0
            });
        }
        
        TargetQuantity.Value = target;
        Exposures = exposures;

        return new ActuatorResponseModels.SetActuatorResponseModel()
        {
            Target = TargetQuantity,
            Exposures = Exposures
        };
    }

    public ActuatorResponseModels.GetActuatorResponseModel Read()
    {
        return new ActuatorResponseModels.GetActuatorResponseModel()
        {
            Current = CurrentQuantity,
            Target = TargetQuantity,
            IsOnTarget = CurrentQuantity.Value.Equals(TargetQuantity.Value),
            Exposures = Exposures
        };
    }
    

    public async Task Update(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var _waiting = DateTime.Now < _waitUntil;
            
            if (_waiting)
            {
                Logger.LogInformation("Waiting... and waiting... and waiting...");
            }
            
            if (Exposures.Count > 0 && !_waiting)
            {
                var exposure = Exposures.Peek();
                var measurement = CurrentQuantity;

                if (measurement.Value.Equals(exposure.Value))
                {
                    Exposures.Dequeue();
                    ValueReachedEvent?.Invoke(this, exposure);
                }
                else
                {
                    var value = Config.MotionFunction.Calculate(measurement.Value, exposure.Value, exposure.Speed);
                    CurrentQuantity.Value = value;
                    ValueChangedEvent?.Invoke(this, exposure);
                }
            }
            
            await Task.Delay(500, stoppingToken);
        }
    }
}