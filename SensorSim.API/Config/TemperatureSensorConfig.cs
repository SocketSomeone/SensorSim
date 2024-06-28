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
    
    public double Inertia { get; } = 0.5;
    
    public Queue<PhysicalValueExposure> Exposures { get; } = new Queue<PhysicalValueExposure>(
        new[]
        {
            new PhysicalValueExposure(10),
            new PhysicalValueExposure(24),
            new PhysicalValueExposure(27),
            new PhysicalValueExposure(24),
            new PhysicalValueExposure(20),
            new PhysicalValueExposure(24),
        }
    );
}