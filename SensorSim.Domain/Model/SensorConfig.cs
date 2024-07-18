using SensorSim.Domain.Enums;

namespace SensorSim.Domain.Model;

public class RandomErrorConfig
{
    public string Type { get; set; } = RandomErrorType.Gaussian;

    public double Mean { get; set; } = 0;

    public double StandardDeviation { get; set; } = 1;
}

public class StaticFunctionConfig
{
    public string Type { get; set; } = StaticFunctionType.Polynomial;


    public List<double> Coefficients { get; set; } = new() { 0, 1 };
}

public class SystematicErrorConfig
{
    public string Type { get; set; } = SystematicErrorType.Constant;

    public double Value { get; set; } = 0;
}

public class SensorConfig(string id) : Entity(id)
{
    public RandomErrorConfig RandomErrorConfig { get; set; } = new();

    public StaticFunctionConfig StaticFunctionConfig { get; set; } = new();

    public SystematicErrorConfig SystematicErrorConfig { get; set; } = new();
    
    public List<double> ApproximateCoefficients { get; set; } = new() { 0, 1 };
}