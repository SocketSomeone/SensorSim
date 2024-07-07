using SensorSim.Domain.DTO.Sensor;

namespace SensorSim.Domain.DTO.Actuator;

public class ActuatorResponseModels
{
    public class CalibrationResponseModel
    {
        public double[,] referenceValues { get; set; }
        
        public double[,] measuredValues { get; set; }
        
        public double[,] errors { get; set; }
        
        public double[,] correction { get; set; }
        
        public bool IsValid { get; set; }
    }

    public class ActuatorResponseModel
    {
        public double Value { get; set; }
        
        public string Unit { get; set; }

        public Queue<PhysicalValueExposure> Exposures { get; set; }
    }
}