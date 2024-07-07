using Microsoft.Extensions.Logging;
using SensorSim.Domain.Interface;

namespace SensorSim.Domain;

public abstract class Sensor<T> : ISensor<T> where T : IPhysicalQuantity
{
    public event ISensor<T>.ArrivalEventHandler? ArrivalEvent;

    public ILogger<ISensor<T>> Logger { get; set; }

    public ISensorConfig<T> Config { get; set; }

    public T CurrentQuantity { get; set; }

    public PhysicalValueExposure Exposure { get; set; }

    public Sensor(ILogger<ISensor<T>> logger, ISensorConfig<T> config)
    {
        Logger = logger;
        Config = config;
        CurrentQuantity = config.InitialQuantity;
        Exposure = new PhysicalValueExposure(CurrentQuantity.Value, 1);
    }

    public double PrimaryConverter(double value)
    {
        return Config.StaticFunction.Calculate(value) +
               Config.SystematicError.Calculate(value) +
               Config.RandomError.Calculate(value);
    }

    public double SecondaryConverter(double value)
    {
        return Config.MotionFunction.Calculate(value, Exposure.Value, Exposure.Duration);
    }

    public T ReadQuantity()
    {
        return (T)Activator.CreateInstance(typeof(T), PrimaryConverter(CurrentQuantity.Value));
    }

    public T UpdateQuantity()
    {
        var parameter = GetParameter();
        var affected = SecondaryConverter(parameter);
        CurrentQuantity.Value = CurrentQuantity.Value + affected - parameter;
        
        if (affected.Equals(Exposure.Value))
        {
            ArrivalEvent?.Invoke(this, Exposure);
        }

        return ReadQuantity();
    }

    public T SetQuantity(double value)
    {
        SetDirection(value, 1);
        CurrentQuantity.Value = value;
        return ReadQuantity();
    }

    public void SetDirection(double destination, double duration)
    {
        SetDirection(new PhysicalValueExposure
        {
            Value = destination,
            Duration = duration
        });
    }

    public void SetDirection(PhysicalValueExposure exposure)
    {
        Exposure = exposure;
    }

    public void Calibrate(List<double> values)
    {
        Config.StaticFunction.SetOptions(values);
    }

    public double GetParameter()
    {
        return PrimaryConverter(CurrentQuantity.Value);
    }

    public double GetAffected()
    {
        return SecondaryConverter(GetParameter());
    }
}