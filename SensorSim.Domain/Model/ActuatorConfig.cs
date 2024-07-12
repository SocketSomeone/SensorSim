namespace SensorSim.Domain.Model;

public class ActuatorConfig(string id) : Entity(id)
{
    public PhysicalQuantity TargetQuantity { get; set; } = new(id);

    public Queue<PhysicalExposure> Exposures { get; set; } = new();

    public DateTime WaitUntil { get; set; } = DateTime.Now;
}