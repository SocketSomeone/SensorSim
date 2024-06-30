using Microsoft.Extensions.Logging;
using SensorSim.Domain.DTO.Actuator;
using SensorSim.Domain.Interface;

namespace SensorSim.Domain.Actuator;

public class Actuator<T> : IActuator<T> where T : IPhysicalQuantity
{
    public ILogger<IActuator<T>> Logger { get; set; }

    public IActuatorConfig<T> Config { get; set; }

    public ISensor<T> Sensor { get; set; }

    public Actuator(ILogger<Actuator<T>> logger, IActuatorConfig<T> config, ISensor<T> sensor)
    {
        Logger = logger;
        Config = config;
        Sensor = sensor;
    }

    public ActuatorResponseModels.CalibrationResponseModel Calibrate(double target,
        Queue<PhysicalValueExposure> exposures)
    {
        List<double> referencedValues = new();
        List<double> measuredValues = new();

        double error = 0;
        double correction = 0;
        
        if (exposures.Count == 0 || exposures.Last().Value != target)
        {
            exposures.Enqueue(new PhysicalValueExposure
            {
                Value = target,
                Duration = 1
            });
        }

        while (exposures.Count > 0)
        {
            var exposure = exposures.Peek();
            var measurement = Sensor.Read();

            Logger.LogInformation($"Exposure: {exposure.Value}, Measurement: {measurement.Value}");
            Sensor.SetDirection(exposure);
            Sensor.Update();
            if (exposure.Value.Equals(measurement.Value))
            {
                exposures.Dequeue();
            }

            referencedValues.Add(exposure.Value);
            measuredValues.Add(measurement.Value);
        }
        
        for (int i = 0; i < referencedValues.Count; i++)
        {
            error += referencedValues[i] - measuredValues[i];
        }
        
        correction = error / referencedValues.Count;


        return new ActuatorResponseModels.CalibrationResponseModel
        {
            referenceValues = referencedValues,
            measuredValues = measuredValues,
            errors = error,
            correction = correction
        };
    }
}