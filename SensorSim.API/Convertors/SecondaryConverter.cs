using SensorSim.Domain;
using SensorSim.Domain.Interface;

namespace SensorSim.API.Convertors;

public class SecondaryConverter : IConvert
{
    public double RateOfChange { get; set; }

    public Queue<PhysicalValueExposure> Exposure { get; set; }

    public SecondaryConverter(double rateOfChange, Queue<PhysicalValueExposure> exposure)
    {
        RateOfChange = rateOfChange;
        Exposure = exposure;
    }

    public double Convert(double value)
    {
        if (Exposure.Count == 0)
        {
            return value;
        }

        var target = Exposure.Peek();
        var difference = target.Value - value;
        var change = RateOfChange * target.TimeStep.TotalSeconds;

        if (IsStable(difference, change))
        {
            Exposure.Dequeue();
            return target.Value;
        }

        return value + Math.Sign(difference) * change;
    }

    public bool IsStable(double difference, double change)
    {
        return Math.Abs(difference) < change;
    }
}