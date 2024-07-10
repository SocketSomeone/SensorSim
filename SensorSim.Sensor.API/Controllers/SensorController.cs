using Microsoft.AspNetCore.Mvc;
using SensorSim.Domain.Interface;
using SensorSim.Domain.DTO.Sensor;

namespace SensorSim.Sensor.API.Controllers;

public abstract class SensorController<T> : ControllerBase where T : IPhysicalQuantity
{
    public ISensor<T> SensorService { get; }

    public SensorController(ISensor<T> sensorService)
    {
        SensorService = sensorService;
    }

    /// <summary>
    /// Get current sensor value
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public ActionResult<SensorsResponseModels.GetSensorResponseModel> Get()
    {
        var quantity = SensorService.ReadQuantity();

        return Ok(new SensorsResponseModels.GetSensorResponseModel
        {
            Current = new SensorsResponseModels.PhysicalQuantity()
            {
                Value = quantity.Value,
                Unit = quantity.Unit
            },
            Parameter = SensorService.ReadParameter()
        });
    }

    /// <summary>
    /// Set sensor value
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost]
    public ActionResult<SensorsResponseModels.SetSensorResponseModel> SetSensorValue(
        [FromBody] SensorsRequestModels.SetSensorValueRequestModel dto)
    {
        SensorService.SetQuantity(dto.Value);
        var quantity = SensorService.ReadQuantity();

        return Ok(new SensorsResponseModels.GetSensorResponseModel
        {
            Current = new SensorsResponseModels.PhysicalQuantity
            {
                Value = quantity.Value,
                Unit = quantity.Unit
            },
            Parameter = SensorService.ReadParameter()
        });
    }
}