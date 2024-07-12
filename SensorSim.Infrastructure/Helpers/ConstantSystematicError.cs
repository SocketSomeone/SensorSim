using SensorSim.Domain.Interface;

namespace SensorSim.Infrastructure.Helpers;

public class ConstantSystematicError(double value) : ISystematicError
{
    private double Value { get; set; } = value;

    public double Calculate(double value)
    {
        return Value;
    }
}