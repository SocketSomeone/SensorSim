namespace SensorSim.Domain.Model;

public class ActuatorEvent(string id) : Entity(id)
{
    public string ActuatorId { get; set; }
    
    public object Value { get; set; }
    
    public string Name { get; set; }
    
    public DateTime CreatedAt { get; } = DateTime.Now;
}