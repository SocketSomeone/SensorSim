using SensorSim.Domain.Interface;

namespace SensorSim.Domain;

public class Pressure : IPhysicalQuantity
{
    public double Value { get; set; }
    
    public string Unit { get; set; } = "Pa";
    
    public Pressure(double value)
    {
        Value = value;
    }
}