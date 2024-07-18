using SensorSim.Domain.Interface;

namespace SensorSim.Infrastructure.Helpers;

public class PolynomialStaticFunction(List<double> coefficients) : IStaticFunction
{
    private List<double> Coefficients { get; } = coefficients;

    public double Calculate(double value)
    {
        return Coefficients.Select((t, i) => t * Math.Pow(value, i)).Sum();
    }
}