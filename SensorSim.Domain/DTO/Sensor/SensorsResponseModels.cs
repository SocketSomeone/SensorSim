using SensorSim.Domain.Interface;

namespace SensorSim.Domain.DTO.Sensor;

public class SensorsResponseModels
{
    public class GetSensorResponseModel
    {
        public IPhysicalQuantity Current { get; set; }

        public double Parameter { get; set; }
    }
}