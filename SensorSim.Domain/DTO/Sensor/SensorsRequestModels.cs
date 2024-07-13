using SensorSim.Domain.Model;

namespace SensorSim.Domain.DTO.Sensor;

public class SetSensorValueRequestModel
{
    public double Value { get; set; }

    public string Unit { get; set; }
}

public class SetSensorConfigRequestModel
{
    public required StaticFunctionConfig StaticFunctionConfig { get; set; }
}