namespace SensorSim.API.Config;

public interface IActuatorConfig
{
    double MinValue { get; set; }
    
    double MaxValue { get; set; }
}