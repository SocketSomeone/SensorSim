using SensorSim.Domain.Interface;

namespace SensorSim.API.Helpers;

public class GaussianRandomError : IRandomError
{
    public double Mean { get; set; }
    
    public double StandardDeviation { get; set; }

    private GaussianDistribution _gaussianDistribution;
    
    public GaussianRandomError(double mean, double standardDeviation)
    {
        Mean = mean;
        StandardDeviation = standardDeviation;
        _gaussianDistribution = new GaussianDistribution(mean, standardDeviation);
    }
    
    public double Emulate(double value)
    {
        return _gaussianDistribution.Next();
    }
}