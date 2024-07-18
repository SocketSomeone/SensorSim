using Newtonsoft.Json.Linq;
using SensorSim.Domain.Enums;
using SensorSim.Domain.Interface;
using SensorSim.Domain.Model;
using SensorSim.Infrastructure.Helpers;
using SensorSim.Infrastructure.Repositories;
using SensorSim.Sensor.API.Interface;

namespace SensorSim.Sensor.API.Services;

public class SensorService(
    ILogger<SensorService> logger,
    CrudMemoryRepository<SensorConfig> sensorConfigsRepository,
    CrudMemoryRepository<PhysicalQuantity> quantitiesRepository)
    : ISensorService
{
    private ILogger<ISensorService> Logger { get; } = logger;

    private CrudMemoryRepository<SensorConfig> SensorConfigsRepository { get; } = sensorConfigsRepository;

    private CrudMemoryRepository<PhysicalQuantity> QuantitiesRepository { get; } = quantitiesRepository;


    public string[] GetSensors()
    {
        return QuantitiesRepository.GetAll().Select(q => q.Id).ToArray();
    }

    public PhysicalQuantity SetQuantity(string id, double value, string unit)
    {
        var quantity = QuantitiesRepository.GetOrDefault(id);
        quantity.Value = value;
        quantity.Unit = unit;
        return quantity;
    }

    public PhysicalQuantity ReadQuantity(string id)
    {
        return QuantitiesRepository.GetOrDefault(id);
    }

    public double ReadParameter(string id)
    {
        var config = SensorConfigsRepository.GetOrDefault(id);
        var quantity = QuantitiesRepository.GetOrDefault(id);

        var randomError = GetRandomError(config);
        var staticFunction = GetStaticFunction(config);
        var systematicError = GetSystematicError(config);

        return staticFunction.Calculate(quantity.Value) +
               systematicError.Calculate(quantity.Value) +
               randomError.Calculate(quantity.Value);
    }

    public double ReadLinearRegression(string id)
    {
        var quantity = QuantitiesRepository.GetOrDefault(id);

        var parameter = ReadParameter(id);

        Logger.LogInformation($"Parameter: {parameter}, Quantity: {quantity.Value}");

        var xData = Enumerable.Range(0, 10).Select(i => quantity.Value).ToArray();
        var yData = Enumerable.Range(0, 10).Select(x => ReadParameter(id)).ToArray();


        // Linear regression for parameter
        double rSquared, intercept, slope;
        LinearRegression(xData, yData, out rSquared, out intercept, out slope);
        // Predict x
        return (double)yData.GetValue(0) * slope + intercept;
    }
    
    public void Delete(string sensorId)
    {
        SensorConfigsRepository.Delete(sensorId);
        QuantitiesRepository.Delete(sensorId);
    }

    public SensorConfig GetConfig(string sensorId)
    {
        return SensorConfigsRepository.GetOrDefault(sensorId);
    }

    private static IRandomError GetRandomError(SensorConfig config)
    {
        var randomConfig = config.RandomErrorConfig;
        var type = randomConfig.Type;

        return type switch
        {
            // Default must be gaussian
            RandomErrorType.Gaussian => new GaussianRandomError(randomConfig.Mean, randomConfig.StandardDeviation),
            _ => throw new ArgumentOutOfRangeException("RandomErrorConfig.Type")
        };
    }

    private static IStaticFunction GetStaticFunction(SensorConfig config)
    {
        var staticConfig = config.StaticFunctionConfig;
        var type = staticConfig.Type;

        return type switch
        {
            // Default must be polynomial
            StaticFunctionType.Polynomial => new PolynomialStaticFunction(staticConfig.Coefficients),
            _ => throw new ArgumentOutOfRangeException("StaticFunctionConfig.Type")
        };
    }

    private static ISystematicError GetSystematicError(SensorConfig config)
    {
        var systematicConfig = config.SystematicErrorConfig;
        var type = systematicConfig.Type;

        return type switch
        {
            SystematicErrorType.Constant => new ConstantSystematicError(systematicConfig.Value),
            _ => throw new ArgumentOutOfRangeException("SystematicErrorConfig.Type")
        };
    }

    private static void LinearRegression(double[] xVals, double[] yVals, out double rSquared, out double yIntercept, out double slope) {
        if (xVals.Length != yVals.Length) {
            throw new Exception("Input values should be with the same length.");
        }

        double sumOfX = 0;
        double sumOfY = 0;
        double sumOfXSq = 0;
        double sumOfYSq = 0;
        double sumCodeviates = 0;

        for (var i = 0; i < xVals.Length; i++) {
            var x = xVals[i];
            var y = yVals[i];
            sumCodeviates += x * y;
            sumOfX += x;
            sumOfY += y;
            sumOfXSq += x * x;
            sumOfYSq += y * y;
        }

        var count = xVals.Length;
        var ssX = sumOfXSq - ((sumOfX * sumOfX) / count);
        var ssY = sumOfYSq - ((sumOfY * sumOfY) / count);

        var rNumerator = (count * sumCodeviates) - (sumOfX * sumOfY);
        var rDenom = (count * sumOfXSq - (sumOfX * sumOfX)) * (count * sumOfYSq - (sumOfY * sumOfY));
        var sCo = sumCodeviates - ((sumOfX * sumOfY) / count);

        var meanX = sumOfX / count;
        var meanY = sumOfY / count;
        var dblR = rNumerator / Math.Sqrt(rDenom);

        rSquared = dblR * dblR;
        yIntercept = meanY - ((sCo / ssX) * meanX);
        slope = sCo / ssX;
    }
}
