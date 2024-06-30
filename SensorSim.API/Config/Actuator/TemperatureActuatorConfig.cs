using SensorSim.Domain;
using SensorSim.Domain.Interface;

namespace SensorSim.API.Config;

public class TemperatureActuatorConfig : IActuatorConfig<Temperature>
{
    public double MinDesiredValue { get; set; } = 0.0;
    
    public double MaxDesiredValue { get; set; } = 100.0;
}