using Microsoft.AspNetCore.Mvc;
using SensorSim.Sensor.API.Services;
using SensorSim.Domain;
using SensorSim.Domain.Interface;

namespace SensorSim.Sensor.API.Controllers;

[ApiController]
[Route("api/sensors/pressure")]
public class PressureSensorController : SensorController<Pressure>
{
    public PressureSensorController(ISensor<Pressure> service) : base(service)
    {
    }
}