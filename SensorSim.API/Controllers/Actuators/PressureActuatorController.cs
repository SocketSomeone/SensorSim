using Microsoft.AspNetCore.Mvc;
using SensorSim.Domain;
using SensorSim.Domain.Interface;

namespace SensorSim.API.Controllers.Actuators;

[ApiController]
[Route("api/actuators/pressure")]
public class PressureActuatorController : ActuatorController<Pressure>
{
    public PressureActuatorController(IActuator<Pressure> actuatorService) : base(actuatorService)
    {
    }
}