using SensorSim.Domain.Model;

namespace SensorSim.Domain.DTO.Sensor;

public class SetSensorValueRequestModel
{
    public double Value { get; set; }

    public string Unit { get; set; }

    public SetSensorValueRequestModel()
    {
    }

    public SetSensorValueRequestModel(double value)
    {
        Value = value;
    }

    public SetSensorValueRequestModel(double value, string unit) : this(value)
    {
        Unit = unit;
    }
}

public class SetSensorConfigRequestModel
{
    public required StaticFunctionConfig StaticFunctionConfig { get; set; }
}