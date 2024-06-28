using SensorSim.Domain.Interface;

namespace SensorSim.Domain;

public class Sensor<T> where T : IPhysicalQuantity
{
    private T Quantity { get; set; }

    private IConvert PrimaryConvert { get; set; }

    private IConvert SecondaryConvert { get; set; }
    
    private SensorMetrics Metrics { get; set; } = new SensorMetrics();

    public Sensor(T quantity, IConvert primaryConvert,
        IConvert secondaryConvert)
    {
        Quantity = quantity;
        PrimaryConvert = primaryConvert;
        SecondaryConvert = secondaryConvert;
    }
    
    public double Parameter => PrimaryConvert.Convert(Quantity.Value);
    
    public double ParameterWithError => SecondaryConvert.Convert(Parameter);

    public void Update()
    {
        var value = ParameterWithError;
        Quantity.Value = value;
        Metrics.Update(value);
    }
    
    public T GetQuantity()
    {
        return Quantity;
    }
    
    public SensorMetrics GetMetrics()
    {
        return Metrics;
    }
}