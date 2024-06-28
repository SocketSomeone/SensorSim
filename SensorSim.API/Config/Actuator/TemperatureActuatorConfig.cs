namespace SensorSim.API.Config;

public class TemperatureActuatorConfig : IActuatorConfig
{
    public double MinValue { get; set; } = 0;
    
    public double MaxValue { get; set; } = 100;
}