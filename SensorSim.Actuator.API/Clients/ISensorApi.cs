using Refit;
using SensorSim.Domain.DTO.Sensor;

namespace SensorSim.Actuator.API.Clients;

public interface ISensorApi
{
    [Get("/api/sensors/{sensorType}")]
    Task<SensorsResponseModels.GetSensorResponseModel> ReadQuantity(string sensorType);
    
    [Post("/api/sensors/{sensorType}")]
    Task<SensorsResponseModels.SetSensorResponseModel> SetQuantity(string sensorType, SensorsRequestModels.SetSensorValueRequestModel request);
}