using SensorSim.Domain;
using SensorSim.Domain.Interface;

namespace SensorSim.API.Models;

public class SensorsModels
{
    public class AddExposureRequest
    {
        public double Value { get; set; }
        public double Duration { get; set; }
    }
    
    public class SensorResponse
    {
        public IPhysicalQuantity quantity { get; set; }
        
        public SensorMetrics metrics { get; set; }
    }
}