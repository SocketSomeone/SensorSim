using SensorSim.Domain.DTO.Actuator;

namespace SensorSim.Domain.Interface;

public interface IActuatorConfig<T> where T : IPhysicalQuantity
{
    public double MaxDeviation { get; set; }
    
    public int NumOfExperiments { get; set; }
    
    public int NumOfMeasurements { get; set; }
    
    public List<double> ReferenceValues { get; set; }
}

public interface IActuator<T> where T : IPhysicalQuantity
{
    public ActuatorResponseModels.ActuatorResponseModel Set(double target, Queue<PhysicalValueExposure> exposures);
    
    public ActuatorResponseModels.CalibrationResponseModel Calibrate();
    public ActuatorResponseModels.ActuatorResponseModel Read();
}