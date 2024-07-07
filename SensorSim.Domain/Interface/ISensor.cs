using Microsoft.Extensions.Logging;

namespace SensorSim.Domain.Interface;

public interface ISensorConfig<T> where T : IPhysicalQuantity
{
    T InitialQuantity { get; }
    
    IStaticFunction StaticFunction { get; }
   
    ISystematicError SystematicError { get; }
   
    IRandomError RandomError { get; }
   
    IMotionFunction MotionFunction { get; }
}

public interface ISensor<T> where T : IPhysicalQuantity
{
    public T UpdateQuantity();
    
    public T ReadQuantity();
    
    public double PrimaryConverter();
    
    public double SecondaryConverter();

    public T SetQuantity(double value);
    
    public void SetDirection(double destination, double speed);
    
    public void SetDirection(PhysicalValueExposure exposure);
    
    public void Calibrate(List<double> values);
}