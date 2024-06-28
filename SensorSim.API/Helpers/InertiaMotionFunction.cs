using SensorSim.Domain;
using SensorSim.Domain.Interface;

namespace SensorSim.API.Helpers;

public class InertiaMotionFunction : IMotionFunction
{
    private double RateOfChange { get; }

    private double Destionation { get; set; }
    
    private double Speed { get; set; }

    public InertiaMotionFunction(double rateOfChange)
    {
        RateOfChange = rateOfChange;
    }
    
    public double Calculate(double value)
    {
        if (Destionation == null || Speed == null)
        {
            Destionation = value;
            Speed = 1;
            return value;
        }
        
        var difference = Destionation - value;
        var change = RateOfChange * Speed;

        if (IsStable(difference, change))
        {
            return Destionation;
        }

        return value + Math.Sign(difference) * change;
    }
    
    public void SetDestination(double destination)
    {
        Destionation = destination;
    }
    
    public void SetSpeed(double speed)
    {
        Speed = speed;
    }
    
    public bool IsStable(double value)
    {
        return IsStable(Destionation - value, RateOfChange * Speed);
    }
    
    public bool IsStable(double difference, double change)
    {
        return Math.Abs(difference) < change;
    }
}