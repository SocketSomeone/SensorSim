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
    

    public SensorService(ISensorConfig<T> config)
    {
        Config = config;
        PrimaryConverter primaryConverter = new PrimaryConverter(Config.StaticFunction, Config.SystematicError, Config.RandomError);
        SecondaryConverter secondaryConverter = new SecondaryConverter(Config.Inertia, Config.Exposures);
        Sensor = new Sensor<T>(Config.DefaultValue, primaryConverter, secondaryConverter);
    }
    
    public SensorsModels.SensorResponse GetCurrentValue()
    {
        Sensor.Update();
        var value = Sensor.GetQuantity();
        var metrics = Sensor.GetMetrics();
        return new SensorsModels.SensorResponse()
        {
            quantity = value,
            metrics = metrics
        };
    }
    
    public void AddExposure(PhysicalValueExposure exposure)
    {
        Config.Exposures.Enqueue(exposure);
    }
    
    public void SetExposures(Queue<PhysicalValueExposure> exposures)
    {
        Config.Exposures.Clear();
        foreach (var exposure in exposures)
        {
            Config.Exposures.Enqueue(exposure);
        }
    }
    
    public void RemoveExposure()
    {
        Config.Exposures.Dequeue();
    }
}