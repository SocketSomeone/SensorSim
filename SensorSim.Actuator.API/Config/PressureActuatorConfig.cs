using SensorSim.Domain;
using SensorSim.Domain.Interface;
using SensorSim.Infrastructure.Helpers;

namespace SensorSim.Actuator.API.Config;

public class PressureActuatorConfig : IActuatorConfig<Pressure>
{
    public Pressure InitialQuantity { get; } = new (0.0);

    public IMotionFunction MotionFunction { get; } = new InertiaMotionFunction(1.0);
}