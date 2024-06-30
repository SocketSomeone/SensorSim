using Microsoft.AspNetCore.Mvc;
using SensorSim.API.Services;
using SensorSim.Domain;
using SensorSim.Domain.Interface;

namespace SensorSim.API.Controllers;

[ApiController]
[Route("api/sensors/pressure")]
public class PressureSensorController : SensorController<Pressure>
{
    public PressureSensorController(ISensor<Pressure> service) : base(service)
    {
    }
}