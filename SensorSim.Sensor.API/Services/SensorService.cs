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


        // TODO: Implement linear regression for parameter
        return 1.0;
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
}