using SensorSim.Domain.Model;

namespace SensorSim.Domain.DTO.Actuator;

public class TargetQuantityRequestModel
{
    public double Value { get; set; }

    public string Unit { get; set; }
}

public class SetActuatorRequestModel
{
    public TargetQuantityRequestModel? CurrentQuantity { get; set; }
    
    public TargetQuantityRequestModel TargetQuantity { get; set; }

    public Queue<PhysicalExposure> Exposures { get; set; }
}