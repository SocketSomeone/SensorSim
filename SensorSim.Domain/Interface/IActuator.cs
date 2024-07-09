using SensorSim.Domain.Actuator;
using SensorSim.Domain.DTO.Actuator;

namespace SensorSim.Domain.Interface;

public interface IActuatorConfig<T> where T : IPhysicalQuantity
{
    public T InitialQuantity { get; }

    public IMotionFunction MotionFunction { get; }
}

public interface IActuator<T> where T : IPhysicalQuantity
{
    public delegate void ValueChangedEventHandler(object sender, PhysicalValueExposure exposure);

    public event ValueChangedEventHandler ValueChangedEvent;

    public delegate void ValueReachedEventHandler(object sender, PhysicalValueExposure exposure);

    public event ValueReachedEventHandler ValueReachedEvent;

    public ActuatorResponseModels.SetActuatorResponseModel Set(double target, Queue<PhysicalValueExposure> exposures);

    public ActuatorResponseModels.GetActuatorResponseModel Read();
    
    Task Update(CancellationToken stoppingToken);
}

public interface IActuatorEvents<T> : IList<ActuatorEvent> where T : IPhysicalQuantity
{
}