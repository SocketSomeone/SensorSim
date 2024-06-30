namespace SensorSim.Domain.Interface;

public interface IPhysicalQuantity
{
    public double Value { get; set; }

    public string Unit { get; set; }
}