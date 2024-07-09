using Microsoft.Extensions.Logging;

namespace SensorSim.Domain.Interface;

public interface ISensorConfig<T> where T : IPhysicalQuantity
{
    T InitialQuantity { get; }

    IStaticFunction StaticFunction { get; }

    ISystematicError SystematicError { get; }

    IRandomError RandomError { get; }
}

public interface ISensor<T> where T : IPhysicalQuantity
{
    public T SetQuantity(double value);
    
    public T ReadQuantity();
    
    public double ReadParameter();

    public double PrimaryConverter(double value);

    public double SecondaryConverter(double value);
}