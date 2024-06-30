using Microsoft.AspNetCore.Mvc;
using SensorSim.API.Services;
using SensorSim.Domain;
using SensorSim.Domain.Interface;

namespace SensorSim.API.Controllers;

[ApiController]
[Route("api/sensors/temperature")]
public class TemperatureSensorController : SensorController<Temperature>
{
    public TemperatureSensorController(ISensor<Temperature> sensorService) : base(sensorService)
    {
    }
}