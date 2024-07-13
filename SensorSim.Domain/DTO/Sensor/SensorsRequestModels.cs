using SensorSim.Domain.Model;

namespace SensorSim.Domain.DTO.Sensor;

public class SetSensorValueRequestModel(double value)
{
    public double Value { get; } = value;

    public string Unit { get; }


    public SetSensorValueRequestModel(double value, string unit) : this(value)
    {
        Unit = unit;
    }
}

public class SetSensorConfigRequestModel
{
    public required StaticFunctionConfig StaticFunctionConfig { get; set; }
}