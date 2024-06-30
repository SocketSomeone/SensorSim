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
    public ILogger<ISensor<T>> Logger { get; set; }
    
    public ISensorConfig<T> Config { get; set; }
    
    public T CurrentQuantity { get; set; }
    
    public T Update();
    
    public T Read();
    
    public double PrimaryConverter();
    
    public double SecondaryConverter();

    public T Set(double value);
    
    public void SetDirection(double destination, double speed);
    
    public void SetDirection(PhysicalValueExposure exposure);
    
    public void Calibrate(List<double> values);
}