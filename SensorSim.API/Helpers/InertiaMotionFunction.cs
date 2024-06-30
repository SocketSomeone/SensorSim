using SensorSim.Domain;
using SensorSim.Domain.Interface;

namespace SensorSim.API.Helpers;

public class InertiaMotionFunction : IMotionFunction
{
    private double RateOfChange { get; }

    public InertiaMotionFunction(double rateOfChange)
    {
        RateOfChange = rateOfChange;
    }
    
    public double Calculate(double value)
    {
        return Calculate(value, value, 1.0);
    }
    
    public double Calculate(double value, double Destionation, double Speed)
    {
        var difference = Destionation - value; 
        var step = RateOfChange * Speed;

        if  (Math.Abs(difference) > step)
        {
            return value + Math.Sign(difference) * step;
        }
        
        return Destionation;
    }
}