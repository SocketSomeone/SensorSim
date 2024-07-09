using SensorSim.API.Helpers;
using SensorSim.Domain;
using SensorSim.Domain.Interface;

namespace SensorSim.API.Config;

public class TemperatureSensorConfig : ISensorConfig<Temperature>
{
    public Temperature InitialQuantity { get; } = new (25.0);
    
    public IStaticFunction StaticFunction { get; } = new PolynomialStaticFunction(new () {3.0, 1.2, 0.125});
    
    public ISystematicError SystematicError { get; } = new ConstantSystematicError(0.2);
    
    public IRandomError RandomError { get; } = new GaussianRandomError(0.0, 0.05);
}