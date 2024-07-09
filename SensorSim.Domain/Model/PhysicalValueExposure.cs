namespace SensorSim.Domain;

public class PhysicalValueExposure
{
    // Value to expose
    public double Value { get; set; }
    
    // Time to keep the value in seconds
    public double Duration { get; set; } = 1;
    
    // Speed of the value change
    public double Speed { get; set; } = 1;
    
    public PhysicalValueExposure()
    {
    }
    
    public PhysicalValueExposure(double value)
    {
        Value = value;
    }
    
    public PhysicalValueExposure(double value, double duration) : this(value)
    {
        Duration = duration;
    }
}