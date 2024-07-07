using SensorSim.Domain;
using SensorSim.Domain.Interface;

namespace SensorSim.API.Config;

public class TemperatureActuatorConfig : IActuatorConfig<Temperature>
{
    public double MaxDeviation { get; set; } = 1.0;
    
    public int NumOfExperiments { get; set; } = 7;
    
    public int NumOfMeasurements { get; set; } = 1;
    
    public List<double> ReferenceValues { get; set; } = new List<double>() { 0, 35, 45, 55, 65, 80, 90 };
}