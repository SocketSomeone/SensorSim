using SensorSim.API.Helpers;
using SensorSim.Domain;
using SensorSim.Domain.Interface;

namespace SensorSim.API.Config;

public class TemperatureSensorConfig : ISensorConfig<Temperature>
{
    public Temperature DefaultValue { get; } = new Temperature(20);
    
    public IStaticFunction StaticFunction { get; } = new PolynomialStaticFunction(new List<double>() { 1, 1, 1 });
    
    public ISystematicError SystematicError { get; } = new ConstantSystematicError(0.1);
    
    public IRandomError RandomError { get; } = new GaussianRandomError(0.0, 1.0);
    
    public IMotionFunction MotionFunction { get; } = new InertiaMotionFunction(0.5);
}