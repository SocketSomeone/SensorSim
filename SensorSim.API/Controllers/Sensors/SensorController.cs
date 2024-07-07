using Microsoft.AspNetCore.Mvc;
using SensorSim.Domain;
using SensorSim.Domain.Interface;
using SensorSim.Domain.DTO.Sensor;

namespace SensorSim.API.Controllers;

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
    public ActionResult<SensorsResponseModels.ISensorResponseModel> Get()
    {
        SensorService.UpdateQuantity();
        return Ok(SensorService.ReadQuantity()); 
    }
    
    /// <summary>
    /// Set sensor value (with inertia)
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost]
    public ActionResult<SensorsResponseModels.ISensorResponseModel> SetTargetSensorValue([FromBody] SensorsRequestModels.SetTargetSensorRequestModel dto)
    {
        SensorService.SetDirection(dto.Value, dto.Duration);
        return Ok(SensorService.ReadQuantity());
    }
    
    /// <summary>
    /// Set sensor value (without inertia)
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPut]
    public ActionResult<SensorsResponseModels.ISensorResponseModel> SetSensorValue([FromBody] SensorsRequestModels.SetSensorValueRequestModel dto)
    {
        return Ok(SensorService.SetQuantity(dto.Value));
    }
}