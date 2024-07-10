using SensorSim.Actuator.API.Clients;
using SensorSim.Domain;
using SensorSim.Domain.Interface;

namespace SensorSim.Actuator.API.Services;

public class PressureActuatorService : ActuatorService<Pressure>
{
    public PressureActuatorService(
        ILogger<PressureActuatorService> logger,
        IActuatorConfig<Pressure> config, ISensorApi sensorApi) : base(logger, config, sensorApi)
    {
    }
}