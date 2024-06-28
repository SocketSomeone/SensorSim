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

        return value;
    }
}