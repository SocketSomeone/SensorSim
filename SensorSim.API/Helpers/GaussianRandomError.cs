using SensorSim.Domain.Interface;

namespace SensorSim.API.Helpers;

public class GaussianRandomError : IRandomError
{
    public double Mean { get; set; } = 0.0;
    
    public double StandardDeviation { get; set; } = 1.0;
    
    private Random _random = new Random();
    
    public GaussianRandomError(double mean, double standardDeviation)
    {
        Mean = mean;
        StandardDeviation = standardDeviation;
    }
    
    public double Calculate(double value)
    {
        var u = 1.0 - _random.NextDouble();
        var v = 1.0 - _random.NextDouble();
        
        var randStdNormal = Math.Sqrt(-2.0 * Math.Log(u)) * Math.Sin(2.0 * Math.PI * v);
        
        return Mean + StandardDeviation * randStdNormal;
    }
}