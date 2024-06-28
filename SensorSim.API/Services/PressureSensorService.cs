using SensorSim.API.Config;
using SensorSim.Domain;

namespace SensorSim.API.Services;

public class PressureSensorService : SensorService<Pressure>
{
    public PressureSensorService(ISensorConfig<Pressure> config) : base(config)
    {
    }
}