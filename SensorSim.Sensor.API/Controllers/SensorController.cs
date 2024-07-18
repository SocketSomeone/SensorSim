using Microsoft.AspNetCore.Mvc;
using SensorSim.Domain.DTO.Sensor;
using SensorSim.Domain.Model;
using SensorSim.Sensor.API.Interface;

namespace SensorSim.Sensor.API.Controllers;

[ApiController]
[Route("api/sensors")]
public class SensorController(ISensorService sensorService) : ControllerBase
{
    private ISensorService SensorService { get; } = sensorService;

    /// <summary>
    /// Get all sensors
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public ActionResult<IEnumerable<GetSensorResponseModel[]>> Get()
    {
        return Ok(SensorService.GetSensors().Select(sensorId =>
        {
            var quantity = SensorService.ReadQuantity(sensorId);
            var parameter = SensorService.ReadParameter(sensorId);

            return new GetSensorResponseModel
            {
                Current = quantity,
                Parameter = parameter,
                ApproximatedValue = SensorService.ReadApproximatedValue(sensorId, parameter)
            };
        }));
    }

    /// <summary>
    /// Get sensor value
    /// </summary>
    /// <param name="sensorId"></param>
    /// <returns></returns>
    [HttpGet("{sensorId}")]
    public ActionResult<GetSensorResponseModel> Get(string sensorId)
    {
        var quantity = SensorService.ReadQuantity(sensorId);
        var parameter = SensorService.ReadParameter(sensorId);

        return Ok(new GetSensorResponseModel
        {
            Current = quantity,
            Parameter = parameter,
            ApproximatedValue = SensorService.ReadApproximatedValue(sensorId, parameter)
        });
    }

    /// <summary>
    /// Set sensor value
    /// </summary>
    /// <param name="sensorId"></param>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost("{sensorId}")]
    public ActionResult<SetSensorResponseModel> SetSensorValue(string sensorId,
        [FromBody] SetSensorValueRequestModel dto)
    {
        var quantity = SensorService.SetQuantity(sensorId, dto.Value, dto.Unit);
        var parameter = SensorService.ReadParameter(sensorId);

        return Ok(new SetSensorResponseModel
        {
            Current = quantity,
            Parameter = parameter
        });
    }
    
    /// <summary>
    /// Delete sensor
    /// </summary>
    /// <param name="sensorId"></param>
    [HttpDelete("{sensorId}")]
    public ActionResult Delete(string sensorId)
    {
        SensorService.Delete(sensorId);
        return Ok();
    }

    /// <summary>
    /// Get sensor config
    /// </summary>
    /// <param name="sensorId"></param>
    /// <returns></returns>
    [HttpGet("{sensorId}/config")]
    public ActionResult<SensorConfig> GetConfig(string sensorId)
    {
        return Ok(SensorService.GetConfig(sensorId));
    }

    /// <summary>
    /// Set sensor config
    /// </summary>
    /// <param name="sensorId"></param>
    /// <param name="config"></param>
    /// <returns></returns>
    [HttpPost("{sensorId}/config")]
    public ActionResult<SensorConfig> SetConfig(string sensorId, [FromBody] SetSensorConfigRequestModel config)
    {
        var sensorConfig = SensorService.GetConfig(sensorId);

        sensorConfig.ApproximateCoefficients = config.ApproximateCoefficients;

        return Ok(sensorConfig);
    }
}