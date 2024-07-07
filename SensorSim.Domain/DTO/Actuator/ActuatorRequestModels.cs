namespace SensorSim.Domain.DTO.Actuator;

public class ActuatorsRequestModels
{
    public class SetRequestModel
    {
        public double Value { get; set; }

        public Queue<PhysicalValueExposure> Exposures { get; set; }
    }
    
    public class CalibrationRequestModel
    {
    }
}