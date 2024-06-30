using SensorSim.Domain;
using SensorSim.Domain.Actuator;
using SensorSim.Domain.Interface;

namespace SensorSim.API.Services.Actuator;

public class TemperatureActuatorService : Actuator<Temperature>
{
    public TemperatureActuatorService(
        ILogger<TemperatureActuatorService> logger,
        IActuatorConfig<Temperature> config,
        ISensor<Temperature> sensorService) : base(logger,
        config, sensorService)
    {
    }
}