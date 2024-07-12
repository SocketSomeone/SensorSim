using SensorSim.Domain.Interface;

namespace SensorSim.Domain.Model;

public class PhysicalQuantity(string id) : Entity(id), IPhysicalQuantity
{
    public double Value { get; set; } = 0;

    public string Unit { get; set; } = "";
}