using SensorSim.Domain.Model;

namespace SensorSim.Domain.DTO.Sensor;

public class GetSensorResponseModel
{
    public PhysicalQuantity Current { get; set; }

    public double Parameter { get; set; }
}

public class SetSensorResponseModel : GetSensorResponseModel
{
}