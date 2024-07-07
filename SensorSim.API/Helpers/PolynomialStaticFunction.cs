using SensorSim.Domain.Interface;

namespace SensorSim.API.Helpers;

public class PolynomialStaticFunction : IStaticFunction
{
    public List<double> Coefficients { get; set; }
    
    public PolynomialStaticFunction(List<double> coefficients)
    {
        Coefficients = coefficients;
    }
    
    public double Calculate(double value)
    {
        double result = 0;
        for (int i = 0; i < Coefficients.Count; i++)
        {
            result += Coefficients[i] * Math.Pow(value, i);
        }

        return result;
    }
    
    public void SetOptions(List<double> values)
    {
        Coefficients = values.Select(Math.Abs).ToList();
    }
}