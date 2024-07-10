namespace SensorSim.Domain.DTO.Sensor;

public class SensorsRequestModels
{
    public class SetSensorValueRequestModel
    {
        public double Value { get; set; }
        
        public SetSensorValueRequestModel()
        {
        }
        
        public SetSensorValueRequestModel(double value)
        {
            Value = value;
        }
    }
}