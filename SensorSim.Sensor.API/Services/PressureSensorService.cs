using SensorSim.Sensor.API.Config;
using SensorSim.Domain;
using SensorSim.Domain.Interface;

namespace SensorSim.Sensor.API.Services;

public class PressureSensorService : Sensor<Pressure>
{
    public PressureSensorService(ILogger<PressureSensorService> logger, ISensorConfig<Pressure> config) : base(logger,
        config)
    {
    }
}