using System.ComponentModel.DataAnnotations;

namespace SensorSim.Domain;

public abstract class Entity
{
    [Key]
    public Guid Id { get; set; }
}