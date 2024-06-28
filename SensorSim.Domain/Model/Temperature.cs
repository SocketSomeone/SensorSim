using SensorSim.Domain.Interface;

namespace SensorSim.Domain;

public class Temperature : IPhysicalQuantity
{
    public double Value { get; set; }
    
    public string Unit { get; set; } = "°C";
    
    public Temperature(double value)
    {
        Value = value;
    }
}