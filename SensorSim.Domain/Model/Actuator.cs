using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SensorSim.Domain.DTO.Actuator;
using SensorSim.Domain.Interface;

namespace SensorSim.Domain.Actuator;

public class Actuator<T> : IActuator<T> where T : IPhysicalQuantity
{
    public ILogger<IActuator<T>> Logger { get; set; }

    public IActuatorConfig<T> Config { get; set; }

    public ISensor<T> Sensor { get; set; }

    public Queue<PhysicalValueExposure> Exposures { get; set; }

    public Actuator(ILogger<Actuator<T>> logger, IActuatorConfig<T> config, ISensor<T> sensor)
    {
        Logger = logger;
        Config = config;
        Sensor = sensor;
        Exposures = new();
    }

    public ActuatorResponseModels.ActuatorResponseModel Set(double target, Queue<PhysicalValueExposure> exposures)
    {
        if (exposures.Count == 0 || !exposures.Last().Value.Equals(target))
        {
            exposures.Enqueue(new PhysicalValueExposure
            {
                Value = target,
                Duration = 1
            });
        }

        Exposures = exposures;
        var measurement = Sensor.ReadQuantity();

        return new ActuatorResponseModels.ActuatorResponseModel()
        {
            Value = measurement.Value,
            Unit = measurement.Unit,
            Exposures = Exposures
        };
    }

    public ActuatorResponseModels.ActuatorResponseModel Read()
    {
        var measurement = Sensor.ReadQuantity();

        if (Exposures.Count > 0)
        {
            var exposure = Exposures.Peek();

            Sensor.SetDirection(exposure);
            Sensor.UpdateQuantity();

            if (exposure.Value.Equals(measurement.Value))
            {
                Exposures.Dequeue();
            }
        }

        Logger.LogInformation($"Read: {measurement.Value} {measurement.Unit}");

        return new ActuatorResponseModels.ActuatorResponseModel()
        {
            Value = measurement.Value,
            Unit = measurement.Unit,
            Exposures = Exposures
        };
    }

    public ActuatorResponseModels.CalibrationResponseModel Calibrate()
    {
        var (X, Y) = RunMeasurements();

        double[,] errors = new double[Config.NumOfExperiments, Config.NumOfMeasurements];
        double[,] correction = new double[Config.NumOfExperiments, Config.NumOfMeasurements];

        var (a, b) = LinearLeastSquares(X, Y);
        var coefficients = new List<double> { b, a };

        for (int i = 0; i < Config.NumOfExperiments; i++)
        {
            for (int j = 0; j < Config.NumOfMeasurements; j++)
            {
                correction[i, j] = X[i, j] / Y[i, j];
                errors[i, j] = Math.Abs(Y[i, j] - X[i, j]);

                Logger.LogInformation($"Measurement: {Y[i, j]}, " +
                                      $"Etalon: {X[i, j]}, " +
                                      $"Error: {errors[i, j]}, Correction: " +
                                      $"{correction[i, j]} " +
                                      $"Actual: {Y[i, j] * correction[i, j]} " +
                                      $"With coefficient: {a * Y[i, j] + b}");
            }
        }

        Sensor.Calibrate(coefficients);
        Logger.LogInformation($"Check: {Check(X, Y)}");


        return new ActuatorResponseModels.CalibrationResponseModel
        {
            referenceValues = X,
            measuredValues = Y,
            errors = errors,
            correction = correction
        };
    }

    private (double, double) LinearLeastSquares(double[,] x, double[,] y)
    {
        double n = Config.NumOfExperiments * Config.NumOfMeasurements;
        double sumX = 0;
        double sumY = 0;
        double sumXY = 0;
        double sumX2 = 0;

        for (int i = 0; i < Config.NumOfExperiments; i++)
        {
            for (int j = 0; j < Config.NumOfMeasurements; j++)
            {
                sumX += x[i, j];
                sumY += y[i, j];
                sumXY += x[i, j] * y[i, j];
                sumX2 += x[i, j] * x[i, j];
            }
        }

        double a = (n * sumXY - sumX * sumY) /
                   (n * sumX2 - sumX * sumX);

        double b = (sumY - a * sumX) / n;

        Logger.LogInformation($"a: {a}, b: {b}");
        return (a, b);
    }

    public IActionResult Approximate()
    {
        var (X, Y) = RunMeasurements();
        var result = Check(X, Y);

        return result ? new OkResult() : new BadRequestResult();
    }

    private (double[,], double[,]) RunMeasurements()
    {
        double[,] X = new double[Config.NumOfExperiments, Config.NumOfMeasurements];
        double[,] Y = new double[Config.NumOfExperiments, Config.NumOfMeasurements];
        Sensor.SetQuantity(Config.ReferenceValues.First());

        for (var i = 0; i < Config.NumOfExperiments; i++)
        {
            var etalonValue = Config.ReferenceValues[i];

            Sensor.SetDirection(etalonValue, 1);

            for (int j = 0; j < Config.NumOfMeasurements; j++)
            {
                Sensor.UpdateQuantity();
                var measurement = Sensor.PrimaryConverter();

                X[i, j] = etalonValue;
                Y[i, j] = measurement;
            }
        }

        return (X, Y);
    }

    private bool Check(double[,] X, double[,] Y)
    {
        double maxDelta = 0;

        for (int i = 0; i < Config.NumOfExperiments; i++)
        {
            for (int j = 0; j < Config.NumOfMeasurements; j++)
            {
                var delta = Math.Abs(X[i, j] - Y[i, j]);
                maxDelta = Math.Max(maxDelta, delta);
            }
        }

        return maxDelta < Config.MaxDeviation;
    }
}