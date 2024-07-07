using SensorSim.Domain;
using SensorSim.Domain.Interface;

namespace SensorSim.API.Config;

public class TemperatureActuatorConfig : IActuatorConfig<Temperature>
{
    public double MaxDeviation { get; set; } = 5.0;
    
    public int NumOfExperiments { get; set; } = 5;

    public int NumOfMeasurements { get; set; } = 2;
    
    public List<double> ReferenceValues { get; set; } = new () { 0.1, 10, 20, 30, 40, 50, 60, 70, 80, 90 };
}