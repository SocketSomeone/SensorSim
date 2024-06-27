using SensorSim.Domain.Interface;

namespace SensorSim.Domain;

public class Sensor
{
    // Volts, Amps, Ohms, Watts, etc.
    public MeasurementQuantity Parameter { get; set; }

    // Temperature, Pressure, Humidity, etc.
    public PhysicalQuantity Quantity { get; set; }

    private IConverter PrimaryConverter { get; set; }

    private IConverter SecondaryConverter { get; set; }

    public Sensor(IConverter primaryConverter, IConverter secondaryConverter) : this(new MeasurementQuantity(),
        new PhysicalQuantity(), primaryConverter, secondaryConverter)
    {
        PrimaryConverter = primaryConverter;
    }

    public Sensor(MeasurementQuantity parameter, PhysicalQuantity quantity, IConverter primaryConverter,
        IConverter secondaryConverter)
    {
        Parameter = parameter;
        Quantity = quantity;
        PrimaryConverter = primaryConverter;
        SecondaryConverter = secondaryConverter;
    }

    public double Measure()
    {
        return PrimaryConverter.Calculate(Parameter.Value);
    }
}