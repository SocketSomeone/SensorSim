using SensorSim.Domain.Model;

namespace SensorSim.Actuator.API.Interface;

public interface IActuatorService
{
    public delegate void ValueChangedEventHandler(object sender, string actuatorId);

    public event ValueChangedEventHandler ValueChangedEvent;

    public delegate void ValueReachedExposureEventHandler(object sender, string actuatorId, PhysicalExposure exposure);

    public event ValueReachedExposureEventHandler ValueReachedExposureEvent;

    string[] GetActuators();

    PhysicalQuantity ReadCurrentQuantity(string id);

    PhysicalQuantity ReadTargetQuantity(string id);

    Queue<PhysicalExposure> ReadExposures(string id);

    Task Update(CancellationToken stoppingToken);
    
    void SetCurrentQuantity(string actuatorId, double value, string unit);
    void SetTargetQuantity(string actuatorId, double value, string unit);
    
    void SetExposures(string actuatorId, Queue<PhysicalExposure> exposures);
    
    IEnumerable<ActuatorEvent> GetEvents(string id);
    
    void Delete(string actuatorId);
}