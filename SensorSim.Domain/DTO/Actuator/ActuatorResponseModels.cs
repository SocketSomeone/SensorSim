using SensorSim.Domain.Interface;
using SensorSim.Domain.Model;

namespace SensorSim.Domain.DTO.Actuator;

public class GetActuatorResponseModel
{
    public IPhysicalQuantity Current { get; set; }

    public IPhysicalQuantity Target { get; set; }

    public bool IsOnTarget { get; set; }

    public Queue<PhysicalExposure> Exposures { get; set; }

    // TODO Add external factors
    public List<IExternalFactor> ExternalFactors { get; set; } = new();
}