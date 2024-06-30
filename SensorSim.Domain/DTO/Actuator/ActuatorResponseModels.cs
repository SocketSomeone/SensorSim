namespace SensorSim.Domain.DTO.Actuator;

public class ActuatorResponseModels
{
    public class CalibrationResponseModel
    {
        public List<double> referenceValues { get; set; }
        
        public List<double> measuredValues { get; set; }
        
        public double errors { get; set; }
        
        public double correction { get; set; }
    }
}