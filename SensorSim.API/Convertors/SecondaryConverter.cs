using SensorSim.Domain;
using SensorSim.Domain.Interface;

namespace SensorSim.API.Convertors;

public class SecondaryConverter : IConvert
{
    IMotionFunction MotionFunction { get; set; }

    public SecondaryConverter(IMotionFunction motionFunction)
    {
        MotionFunction = motionFunction;
    }

    public double Convert(double value)
    {
        return MotionFunction.Calculate(value);
    }
}