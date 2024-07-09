using SensorSim.Domain;
using SensorSim.Domain.Actuator;
using SensorSim.Domain.Interface;

namespace SensorSim.API.Services.Actuator;

public class PressureActuatorService : Actuator<Pressure>
{
    public PressureActuatorService(
        ILogger<PressureActuatorService> logger,
        IActuatorConfig<Pressure> config,
        ISensor<Pressure> sensorService) : base(logger, config, sensorService)
    {
    }
}