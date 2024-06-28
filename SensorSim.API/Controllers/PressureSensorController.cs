using Microsoft.AspNetCore.Mvc;
using SensorSim.API.Services;
using SensorSim.Domain;

namespace SensorSim.API.Controllers;

[ApiController]
[Route("api/sensors/pressure")]
public class PressureSensorController : SensorController<Pressure>
{
    public PressureSensorController(ISensorService<Pressure> service) : base(service)
    {
    }
}