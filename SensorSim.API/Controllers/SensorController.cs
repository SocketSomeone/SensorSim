using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using SensorSim.API.Models;
using SensorSim.API.Services;
using SensorSim.Domain;
using SensorSim.Domain.Interface;

namespace SensorSim.API.Controllers;

public interface ISensorController<T> where T : IPhysicalQuantity
{
    ISensorService<T> SensorService { get; }
}

public abstract class SensorController<T> : ControllerBase where T : IPhysicalQuantity
{
    public ISensorService<T> SensorService { get; }
    
    public SensorController(ISensorService<T> sensorService)
    {
        SensorService = sensorService;
    }
    
    /// <summary>
    /// Get current sensor value
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public ActionResult<SensorsModels.SensorResponse> Get()
    {
        return Ok(SensorService.GetCurrentValue());
    }
    
    /// <summary>
    /// Add exposure to sensor
    /// </summary>
    /// <param name="exposure"></param>
    /// <returns></returns>
    [HttpPost("exposures")]
    public IActionResult AddExposure(SensorsModels.AddExposureRequest exposure)
    {
        SensorService.AddExposure(new PhysicalValueExposure(exposure.Value, TimeSpan.FromSeconds(exposure.Duration)));
        return Ok();
    }
    
    /// <summary>
    /// Set exposures to sensor
    /// </summary>
    /// <param name="exposures"></param>
    /// <returns></returns>
    [HttpPut("exposures")]
    public IActionResult SetExposures(SensorsModels.AddExposureRequest[] exposures)
    {
        Queue<PhysicalValueExposure> physicalValueExposures = new Queue<PhysicalValueExposure>();
        foreach (var exposure in exposures)
        {
            physicalValueExposures.Enqueue(new PhysicalValueExposure(exposure.Value, TimeSpan.FromSeconds(exposure.Duration)));
        }
        SensorService.SetExposures(physicalValueExposures);
        return Ok();
    }
    
    /// <summary>
    ///  Remove exposure from sensor
    /// </summary>
    /// <returns></returns>
    [HttpDelete("exposures")]
    public IActionResult RemoveExposure()
    {
        SensorService.RemoveExposure();
        return Ok();
    }
}