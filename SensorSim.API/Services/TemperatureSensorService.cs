using SensorSim.API.Config;
using SensorSim.Domain;

namespace SensorSim.API.Services;

public class TemperatureSensorService : SensorService<Temperature>
{
    public TemperatureSensorService(ISensorConfig<Temperature> config) : base(config)
    {
    }
}