using Refit;
using SensorSim.Domain.DTO.Sensor;

namespace SensorSim.Actuator.API.Interface;

public interface ISensorApi
{
    [Get("/api/sensors/{sensorId}")]
    Task<GetSensorResponseModel> ReadQuantity(string sensorId);

    [Post("/api/sensors/{sensorId}")]
    Task<SetSensorResponseModel> SetQuantity(string sensorId, [Body] SetSensorValueRequestModel request);

    [Delete("/api/sensors/{actuatorId}")]
    Task Delete(string actuatorId);
}