using Microsoft.AspNetCore.Mvc;
using SensorSim.Sensor.API.Services;
using SensorSim.Domain;
using SensorSim.Domain.Interface;

namespace SensorSim.Sensor.API.Controllers;

[ApiController]
[Route("api/sensors/temperature")]
public class TemperatureSensorController : SensorController<Temperature>
{
    public TemperatureSensorController(ISensor<Temperature> sensorService) : base(sensorService)
    {
    }
}