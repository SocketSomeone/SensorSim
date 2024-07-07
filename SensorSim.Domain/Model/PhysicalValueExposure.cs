namespace SensorSim.Domain;

public class PhysicalValueExposure
{
    public double Value { get; set; }
    
    public double Duration { get; set; } = 1;
    
    public PhysicalValueExposure()
    {
    }
    
    public PhysicalValueExposure(double value)
    {
        Value = value;
    }
    
    public PhysicalValueExposure(double value, TimeSpan timeStep) : this(value)
    {
        Duration = timeStep.TotalSeconds;
    }
    
    public PhysicalValueExposure(double value, double duration)
    {
        Value = value;
        Duration = duration;
    }
}