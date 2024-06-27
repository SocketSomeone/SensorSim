namespace SensorSim.Domain;

public class PhysicalQuantity
{
    public double Value { get; set; } = 0;

    public string Symbol { get; set; } = string.Empty;
}

public class Temperature : PhysicalQuantity
{
    public string Symbol { get; set; } = "°C";
}