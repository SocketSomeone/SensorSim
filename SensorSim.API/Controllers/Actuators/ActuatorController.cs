using Microsoft.AspNetCore.Mvc;
using SensorSim.Domain.DTO.Actuator;
using SensorSim.Domain.Interface;

namespace SensorSim.API.Controllers.Actuators;

public abstract class ActuatorController<T> : ControllerBase where T : IPhysicalQuantity
{
    public IActuator<T> ActuatorService { get; set; }

    public ILogger<ActuatorController<T>> Logger { get; set; }

    public ActuatorController(IActuator<T> actuatorService)
    {
        ActuatorService = actuatorService;
    }


    /// <summary>
    /// Set the value of the actuator
    /// </summary>
    /// <param name="setModel"></param>
    /// <returns></returns>
    [HttpPost]
    public ActionResult<ActuatorResponseModels.SetActuatorResponseModel> Set(
        [FromBody] ActuatorsRequestModels.SetRequestModel setModel)
    {
        return Ok(ActuatorService.Set(setModel.Value, setModel.Exposures));
    }

    /// <summary>
    /// Get the current value of the actuator
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(ActuatorService.Read());
    }
}