using SensorSim.API.Helpers;
using SensorSim.Domain;
using SensorSim.Domain.Interface;

namespace SensorSim.API.Config;

public class TemperatureSensorConfig : ISensorConfig<Temperature>
{
    public Temperature InitialQuantity { get; } = new Temperature(25.0);
    
    public IStaticFunction StaticFunction { get; } = new PolynomialStaticFunction(new List<double>() {0, 1.0});
    
    public ISystematicError SystematicError { get; } = new ConstantSystematicError(1.0);
    
    public IRandomError RandomError { get; } = new GaussianRandomError(0.0, 1.0);
    
    public IMotionFunction MotionFunction { get; } = new InertiaMotionFunction(1.0);
}