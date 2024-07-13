using Microsoft.AspNetCore.Mvc;
using SensorSim.Actuator.API.Clients;
using SensorSim.Actuator.API.Services;
using SensorSim.Domain.DTO.Actuator;
using SensorSim.Domain.Interface;
using SensorSim.Domain.Model;

namespace SensorSim.Actuator.API.Controllers;

/// <summary>
/// Controller for the actuator
/// </summary>
/// <param name="actuatorService"></param>
[ApiController]
[Route("api/actuators")]
public class ActuatorController(IActuatorService actuatorService)
    : ControllerBase
{
    private IActuatorService ActuatorService { get; } = actuatorService;

    [HttpGet]
    public ActionResult<IEnumerable<GetActuatorResponseModel>> GetAll()
    {
        return Ok(ActuatorService.GetActuators().Select(actuatorId => new GetActuatorResponseModel
        {
            Current = ActuatorService.ReadCurrentQuantity(actuatorId),
            Target = ActuatorService.ReadTargetQuantity(actuatorId),
            IsOnTarget = ActuatorService.ReadCurrentQuantity(actuatorId).Value
                .Equals(ActuatorService.ReadTargetQuantity(actuatorId).Value),
            Exposures = ActuatorService.ReadExposures(actuatorId),
            ExternalFactors = []
        }));
    }

    /// <summary>
    /// Set the value of the actuator
    /// </summary>
    /// <param name="actuatorId"></param>
    /// <param name="setActuatorModel"></param>
    /// <returns></returns>
    [HttpPost("{actuatorId}")]
    public ActionResult<GetActuatorResponseModel> Set(
        string actuatorId,
        [FromBody] SetActuatorRequestModel setActuatorModel)
    {
        var targetQuantity = setActuatorModel.TargetQuantity;
        var exposures = setActuatorModel.Exposures;
        
        if (setActuatorModel.CurrentQuantity != null)
        {
            ActuatorService.SetCurrentQuantity(actuatorId, setActuatorModel.CurrentQuantity.Value, setActuatorModel.CurrentQuantity.Unit);
        }

        if (exposures.Count == 0 || !exposures.Last().Value.Equals(targetQuantity.Value))
        {
            exposures.Enqueue(new PhysicalExposure
            {
                Value = targetQuantity.Value,
                Duration = 1,
                Speed = 1.0
            });
        }
        
        ActuatorService.SetTargetQuantity(actuatorId, targetQuantity.Value, targetQuantity.Unit);
        ActuatorService.SetExposures(actuatorId, exposures);
        var current = ActuatorService.ReadCurrentQuantity(actuatorId);
        var target = ActuatorService.ReadTargetQuantity(actuatorId);

        return Ok(new GetActuatorResponseModel()
        {
            Current = current,
            Target = target,
            IsOnTarget = current.Value.Equals(target.Value),
            Exposures = ActuatorService.ReadExposures(actuatorId),
            ExternalFactors = []
        });
    }

    /// <summary>
    /// Get the current value of the actuator
    /// </summary>
    /// <returns></returns>
    [HttpGet("{actuatorId}")]
    public ActionResult<GetActuatorResponseModel> Get(string actuatorId)
    {
        var current = ActuatorService.ReadCurrentQuantity(actuatorId);
        var target = ActuatorService.ReadTargetQuantity(actuatorId);

        return Ok(new GetActuatorResponseModel()
        {
            Current = current,
            Target = target,
            IsOnTarget = current.Value.Equals(target.Value),
            Exposures = ActuatorService.ReadExposures(actuatorId),
            ExternalFactors = []
        });
    }
}