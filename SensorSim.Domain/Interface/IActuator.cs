using Microsoft.Extensions.Logging;
using SensorSim.Domain.DTO.Actuator;

namespace SensorSim.Domain.Interface;

public interface IActuatorConfig<T> where T : IPhysicalQuantity
{
}

public interface IActuator<T> where T : IPhysicalQuantity
{
    public ILogger<IActuator<T>> Logger { get; set; }
    
    public IActuatorConfig<T> Config { get; set; }
    
    public ISensor<T> Sensor { get; set; }
    
    public ActuatorResponseModels.CalibrationResponseModel Calibrate(double target, Queue<PhysicalValueExposure> exposures);
}