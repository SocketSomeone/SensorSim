using SensorSim.Domain;

namespace SensorSim.Infrastructure.Tests.Fixtures;

public class TestEntity : Entity
{
    public string Name { get; set; }

    public TestEntity(string id) : base(id)
    {
    }
    
    public TestEntity(string id, string name) : base(id)
    {
        Name = name;
    }
}