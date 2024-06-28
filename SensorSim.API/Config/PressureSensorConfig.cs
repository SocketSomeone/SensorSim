using SensorSim.API.Helpers;
using SensorSim.Domain;
using SensorSim.Domain.Interface;

namespace SensorSim.API.Config;

public class PressureSensorConfig : ISensorConfig<Pressure>
{
    public Pressure DefaultValue { get; } = new Pressure(100);

    public IStaticFunction StaticFunction { get; } =
        new PolynomialStaticFunction(new List<double>() { 0.5, -0.25, 0.125 });

    public ISystematicError SystematicError { get; } = new ConstantSystematicError(0.05);

    public IRandomError RandomError { get; } = new GaussianRandomError(-0.02, 0.03);

    public double Inertia { get; } = 0.05;

    public Queue<PhysicalValueExposure> Exposures { get; } = new Queue<PhysicalValueExposure>(
        new[]
        {
            new PhysicalValueExposure(50), // Новые начальные условия
            new PhysicalValueExposure(70),
            new PhysicalValueExposure(80),
            new PhysicalValueExposure(60),
            new PhysicalValueExposure(75),
            new PhysicalValueExposure(85),
        }
    );
}