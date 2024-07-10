using SensorSim.Domain.Interface;

namespace SensorSim.Domain.DTO.Sensor;

public class SensorsResponseModels
{
    public class GetSensorResponseModel
    {
        public PhysicalQuantity Current { get; set; }

        public double Parameter { get; set; }
    }
    
    public class SetSensorResponseModel : SensorsResponseModels
    {
    }
    
    public class PhysicalQuantity : IPhysicalQuantity
    {
        public double Value { get; set; }
        
        public string Unit { get; set; }
    }
}