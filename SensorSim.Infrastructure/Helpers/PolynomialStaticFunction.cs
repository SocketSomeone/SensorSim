using SensorSim.Domain.Interface;

namespace SensorSim.Infrastructure.Helpers;

public class PolynomialStaticFunction(List<double> coefficients) : IStaticFunction
{
    private List<double> Coefficients { get; } = coefficients;

    public double Calculate(double value)
    {
        double result = 0;
        for (int i = 0; i < Coefficients.Count; i++)
        {
            result += Coefficients[i] * Math.Pow(value, i);
        }

        return result;
    }
}