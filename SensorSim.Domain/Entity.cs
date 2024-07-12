namespace SensorSim.Domain;

public abstract class Entity
{
    public string Id { get; set; }

    protected Entity()
    {
        Id = Guid.NewGuid().ToString();
    }

    protected Entity(string id)
    {
        Id = id;
    }
}