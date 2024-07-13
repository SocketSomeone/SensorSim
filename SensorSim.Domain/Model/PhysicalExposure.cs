namespace SensorSim.Domain.Model;

public class PhysicalExposure
{
    // Value to expose
    public double Value { get; set; }
    
    // Time to keep the value in seconds
    public double Duration { get; set; } = 1;
    
    // Speed of the value change
    public double Speed { get; set; } = 1;
}