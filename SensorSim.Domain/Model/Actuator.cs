using Microsoft.Extensions.Logging;
using SensorSim.Domain.DTO.Actuator;
using SensorSim.Domain.Interface;

namespace SensorSim.Domain.Actuator;

public class Actuator<T> : IActuator<T> where T : IPhysicalQuantity
{
    public ILogger<IActuator<T>> Logger { get; set; }

    public IActuatorConfig<T> Config { get; set; }

    public ISensor<T> Sensor { get; set; }

    public T ReferenceQuantity { get; set; }

    public Actuator(ILogger<Actuator<T>> logger, IActuatorConfig<T> config, ISensor<T> sensor)
    {
        Logger = logger;
        Config = config;
        Sensor = sensor;
        ReferenceQuantity = (T)Activator.CreateInstance(typeof(T), Sensor.Read().Value);
    }

    public ActuatorResponseModels.CalibrationResponseModel Calibrate(double target,
        Queue<PhysicalValueExposure> exposures)
    {
        if (exposures.Count == 0 || !exposures.Last().Value.Equals(target))
        {
            exposures.Enqueue(new PhysicalValueExposure
            {
                Value = target,
                Duration = 1
            });
        }

        List<MeasurementOfImpact> impacts = Measurement(exposures);

        // Calculate error rate and correction
        double error = impacts.Sum(x => x.MeasuredValues.Average() - x.DesiredValue);
        double correctionFactor = error / impacts.Count;

        // Calculate static function characteristics for polynomial regression
        double maxDesiredValue = Config.MaxDesiredValue;
        double minDesiredValue = Config.MinDesiredValue;

        double maxMeasuredValue = impacts.Max(x => x.DesiredValue);
        double minMeasuredValue = impacts.Min(x => x.DesiredValue);

        
        double slope = (maxMeasuredValue - minMeasuredValue) / (maxDesiredValue - minDesiredValue);
        double intercept = minMeasuredValue - slope * minDesiredValue;
        
        Logger.LogInformation($"Calibration: Slope = {slope}, Intercept = {intercept}");

        Sensor.Calibrate(new List<double>() { intercept, slope });


        return new ActuatorResponseModels.CalibrationResponseModel
        {
            referenceValues = new List<double>(),
            measuredValues = new List<double>(),
            errors = error,
            correction = correctionFactor
        };
    }

    private List<MeasurementOfImpact> Measurement(Queue<PhysicalValueExposure> exposures)
    {
        List<MeasurementOfImpact> impacts = new();

        var exposure = exposures.Peek();
        var measurement = Sensor.Read();
        var currentImpact = new MeasurementOfImpact(exposure.Value);

        while (exposures.Count > 0)
        {
            exposure = exposures.Peek();
            measurement = Sensor.Read();

            if (currentImpact.DesiredValue != exposure.Value)
            {
                currentImpact = new MeasurementOfImpact(exposure.Value);
            }

            Sensor.SetDirection(exposure);
            Sensor.Update();

            currentImpact.MeasuredValues.Add(measurement.Value);

            if (exposure.Value.Equals(measurement.Value))
            {
                exposures.Dequeue();
                impacts.Add(currentImpact);
            }
        }

        return impacts;
    }

    private class MeasurementOfImpact
    {
        public double DesiredValue { get; set; }

        public List<double> MeasuredValues { get; set; }

        public MeasurementOfImpact(double desiredValue)
        {
            DesiredValue = desiredValue;
            MeasuredValues = new();
        }
    }
}