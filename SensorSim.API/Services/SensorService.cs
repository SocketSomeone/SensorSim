using SensorSim.API.Config;
using SensorSim.API.Convertors;
using SensorSim.API.Models;
using SensorSim.Domain;
using SensorSim.Domain.Interface;

namespace SensorSim.API.Services;

public interface ISensorService<T> where T : IPhysicalQuantity
{
    ISensorConfig<T> Config { get; }

    Sensor<T> Sensor { get; }

    SensorsModels.SensorResponse GetCurrentValue();

    void AddExposure(PhysicalValueExposure exposure);

    void SetExposures(Queue<PhysicalValueExposure> exposures);

    void RemoveExposure();
}

public abstract class SensorService<T> : ISensorService<T> where T : IPhysicalQuantity
{
    public Sensor<T> Sensor { get; }
    public ISensorConfig<T> Config { get; }

    public Queue<PhysicalValueExposure> Exposures { get; set; } = new Queue<PhysicalValueExposure>();
    
    public SensorService(ISensorConfig<T> config)
    {
        Config = config;
        PrimaryConverter primaryConverter =
            new PrimaryConverter(Config.StaticFunction, Config.SystematicError, Config.RandomError);
        SecondaryConverter secondaryConverter = new SecondaryConverter(Config.MotionFunction);
        Sensor = new Sensor<T>(Config.DefaultValue, primaryConverter, secondaryConverter);
    }

    public SensorsModels.SensorResponse GetCurrentValue()
    {
        var quantity = Sensor.GetQuantity();
        var metrics = Sensor.GetMetrics();
        
        if (Exposures.Count > 0)
        {
            var exposure = Exposures.Peek();
            
            Config.MotionFunction.SetDestination(exposure.Value);
            Config.MotionFunction.SetSpeed(exposure.TimeStep.TotalSeconds);
            
            if (Config.MotionFunction.IsStable(quantity.Value) )
            {
                Exposures.Dequeue();
            }   
        }
        
        Sensor.Update();
        
        return new SensorsModels.SensorResponse()
        {
            quantity = quantity,
            metrics = metrics
        };
    }

    public void AddExposure(PhysicalValueExposure exposure)
    {
        Exposures.Enqueue(exposure);
    }

    public void SetExposures(Queue<PhysicalValueExposure> exposures)
    {
        Exposures.Clear();
        foreach (var exposure in exposures)
        {
            Exposures.Enqueue(exposure);
        }
    }

    public void RemoveExposure()
    {
        Exposures.Dequeue();
    }
}