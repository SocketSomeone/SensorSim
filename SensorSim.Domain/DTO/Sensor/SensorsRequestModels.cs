namespace SensorSim.Domain.DTO.Sensor;

public class SensorsRequestModels
{
    public class SetSensorValueRequestModel
    {
        public double Value { get; set; }
    }

    public class SetTargetSensorRequestModel : PhysicalValueExposure
    {
    }
}