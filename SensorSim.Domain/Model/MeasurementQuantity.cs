namespace SensorSim.Domain;

public class MeasurementQuantity
{
    public double Value { get; set; } = 0;
    
    public string Symbol { get; set; } = string.Empty;
}

public class Volt : MeasurementQuantity
{
    public string Symbol { get; set; } = "V";
}

public class Ampere : MeasurementQuantity
{
    public string Symbol { get; set; } = "A";
}