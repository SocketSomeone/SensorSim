using Microsoft.AspNetCore.Mvc;
using SensorSim.API.Services;
using SensorSim.Domain;

namespace SensorSim.API.Controllers;

[ApiController]
[Route("api/sensors/temperature")]
public class TemperatureSensorController : SensorController<Temperature>
{
    public TemperatureSensorController(ISensorService<Temperature> sensorService) : base(sensorService)
    {
    }
}