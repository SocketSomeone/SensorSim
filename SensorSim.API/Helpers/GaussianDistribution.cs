using SensorSim.Domain.Interface;

namespace SensorSim.API.Helpers;

public class GaussianDistribution : IDistribution
{
    public double Mean { get; set; } = 0.0;
    
    public double StandardDeviation { get; set; } = 1.0;
    
    private Random _random = new Random();
    
    public GaussianDistribution(double mean, double standardDeviation)
    {
        Mean = mean;
        StandardDeviation = standardDeviation;
    }
    
    public double Next()
    {
        var u1 = 1.0 - _random.NextDouble();
        var u2 = 1.0 - _random.NextDouble();
        var randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
        return Mean + StandardDeviation * randStdNormal;
    }
}