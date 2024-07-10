namespace SensorSim.Domain.DTO.Actuator;

public class ActuatorsRequestModels
{
    public class SetActuatorRequestModel
    {
        public double Value { get; set; }

        public Queue<PhysicalValueExposure> Exposures { get; set; }
    }
}