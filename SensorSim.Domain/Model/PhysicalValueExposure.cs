namespace SensorSim.Domain;

public class PhysicalValueExposure
{
    public double Value { get; set; }
    
    public TimeSpan TimeStep { get; set; } = TimeSpan.FromSeconds(1);
    
    public PhysicalValueExposure(double value)
    {
        Value = value;
    }
    
    public PhysicalValueExposure(double value, TimeSpan timeStep) : this(value)
    {
        TimeStep = timeStep;
    }
}