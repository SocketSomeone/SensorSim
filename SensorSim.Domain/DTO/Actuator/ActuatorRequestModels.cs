namespace SensorSim.Domain.DTO.Actuator;

public class ActuatorsRequestModels
{
    public class CalibrationRequestModel
    {
        public double Value { get; set; }

        public Queue<PhysicalValueExposure> Exposures { get; set; }
    }
}