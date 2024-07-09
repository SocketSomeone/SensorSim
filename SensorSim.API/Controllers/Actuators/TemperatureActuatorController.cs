using Microsoft.AspNetCore.Mvc;
using SensorSim.Domain;
using SensorSim.Domain.Interface;

namespace SensorSim.API.Controllers.Actuators;

[Controller]
[Route("api/actuators/temperature")]
public class TemperatureActuatorController : ActuatorController<Temperature>
{
    public TemperatureActuatorController(IActuator<Temperature> actuatorService) : base(actuatorService)
    {
    }
}