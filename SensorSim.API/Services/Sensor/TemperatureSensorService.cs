using SensorSim.API.Config;
using SensorSim.Domain;
using SensorSim.Domain.Interface;

namespace SensorSim.API.Services;

public class TemperatureSensorService : Sensor<Temperature>
{
    public TemperatureSensorService(ILogger<TemperatureSensorService> logger, ISensorConfig<Temperature> config) :
        base(logger, config)
    {
    }
}