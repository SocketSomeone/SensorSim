using SensorSim.Domain.Interface;

namespace SensorSim.API.Helpers;

public class ConstantSystematicError: ISystematicError
{
    public double Value { get; set; }
    
    public ConstantSystematicError(double value)
    {
        Value = value;
    }
    
    public double Calculate(double value)
    {
        return Value;
    }
}