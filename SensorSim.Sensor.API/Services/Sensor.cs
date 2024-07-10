using SensorSim.Domain.Interface;

namespace SensorSim.Sensor.API.Services;

public abstract class Sensor<T> : ISensor<T> where T : IPhysicalQuantity
{
    public ILogger<ISensor<T>> Logger { get; set; }

    public ISensorConfig<T> Config { get; set; }

    public T CurrentQuantity { get; set; }

    public Sensor(ILogger<ISensor<T>> logger, ISensorConfig<T> config)
    {
        Logger = logger;
        Config = config;
        CurrentQuantity = (T)Activator.CreateInstance(typeof(T), Config.InitialQuantity.Value);
    }

    public double PrimaryConverter(double value)
    {
        return Config.StaticFunction.Calculate(value) +
               Config.SystematicError.Calculate(value) +
               Config.RandomError.Calculate(value);
    }

    public double SecondaryConverter(double value)
    {
        // TODO: Implement linear regression for parameter
        return 1.0;
    }

    public T SetQuantity(double value)
    {
        CurrentQuantity.Value = value;
        return ReadQuantity();
    }

    public T ReadQuantity()
    {
        return CurrentQuantity;
    }

    public double ReadParameter()
    {
        return PrimaryConverter(CurrentQuantity.Value);
    }
}