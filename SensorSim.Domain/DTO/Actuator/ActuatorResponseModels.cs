using SensorSim.Domain.DTO.Sensor;
using SensorSim.Domain.Interface;

namespace SensorSim.Domain.DTO.Actuator;

public class ActuatorResponseModels
{
    public class SetActuatorResponseModel
    {
        public IPhysicalQuantity Target { get; set; }

        public Queue<PhysicalValueExposure> Exposures { get; set; }
    }

    public class GetActuatorResponseModel
    {
        public IPhysicalQuantity Current { get; set; }

        public IPhysicalQuantity Target { get; set; }

        public bool IsOnTarget { get; set; }

        public Queue<PhysicalValueExposure> Exposures { get; set; }

        // TODO Add external factors
        public List<IExternalFactor> ExternalFactors { get; set; } = new();
        // public List<>
    }
}