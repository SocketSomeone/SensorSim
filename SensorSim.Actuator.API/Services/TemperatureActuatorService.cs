using SensorSim.Actuator.API.Clients;
using SensorSim.Domain;
using SensorSim.Domain.Interface;

namespace SensorSim.Actuator.API.Services;

public class TemperatureActuatorService : ActuatorService<Temperature>
{
    public TemperatureActuatorService(
        ILogger<TemperatureActuatorService> logger,
        IActuatorConfig<Temperature> config,
        ISensorApi sensorApi) : base(logger, config, sensorApi)
    {
    }
}