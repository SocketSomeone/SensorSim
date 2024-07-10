using SensorSim.Domain;
using SensorSim.Domain.Interface;
using SensorSim.Infrastructure.Helpers;

namespace SensorSim.Actuator.API.Config;

public class TemperatureActuatorConfig : IActuatorConfig<Temperature>
{
    public Temperature InitialQuantity { get; } = new (25.0);

    public IMotionFunction MotionFunction { get; } = new InertiaMotionFunction(1.0);
}