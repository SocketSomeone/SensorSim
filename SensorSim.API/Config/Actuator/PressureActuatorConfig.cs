using SensorSim.API.Helpers;
using SensorSim.Domain;
using SensorSim.Domain.Interface;

namespace SensorSim.API.Config;

public class PressureActuatorConfig : IActuatorConfig<Pressure>
{
    public Pressure InitialQuantity { get; } = new (0.0);

    public IMotionFunction MotionFunction { get; } = new InertiaMotionFunction(1.0);
}