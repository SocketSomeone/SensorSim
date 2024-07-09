namespace SensorSim.Domain.Interface;

public interface IExternalFactor
{
    public string Name { get; set; }
    
    public double Calculate(double value);
}