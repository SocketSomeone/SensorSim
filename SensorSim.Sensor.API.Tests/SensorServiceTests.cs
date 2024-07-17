using Microsoft.Extensions.Logging;
using Moq;
using SensorSim.Domain.Enums;
using SensorSim.Domain.Model;
using SensorSim.Infrastructure.Repositories;
using SensorSim.Sensor.API.Services;

namespace SensorSim.Sensor.API.Tests;

public class SensorServiceTests
{
    private readonly SensorService _sensorService;
    private readonly Mock<ILogger<SensorService>> _mockLogger;
    private readonly CrudMemoryRepository<SensorConfig> _sensorConfigsRepository;
    private readonly CrudMemoryRepository<PhysicalQuantity> _quantitiesRepository;

    public SensorServiceTests()
    {
        _mockLogger = new Mock<ILogger<SensorService>>();
        _sensorConfigsRepository = new SensorConfigsRepository();
        _quantitiesRepository = new QuantitiesRepository();
        _sensorService = new SensorService(_mockLogger.Object, _sensorConfigsRepository, _quantitiesRepository);
    }

    [Fact]
    public void GetSensors_ShouldReturnAllSensorIds()
    {
        // Arrange
        var quantities = new List<PhysicalQuantity>
        {
            new PhysicalQuantity("sensor1") {  Value = 10, Unit = "Celsius" },
            new PhysicalQuantity("sensor2") { Value = 20, Unit = "Celsius" }
        };
        foreach (var quantity in quantities)
        {
            _quantitiesRepository.Add(quantity);
        }

        // Act
        var sensorIds = _sensorService.GetSensors();

        // Assert
        Assert.Equal(quantities.Select(q => q.Id).ToArray(), sensorIds);
    }

    [Fact]
    public void SetQuantity_ShouldUpdateQuantity()
    {
        // Arrange
        var id = "sensor1";
        var initialQuantity = new PhysicalQuantity(id) { Value = 10, Unit = "Celsius" };
        _quantitiesRepository.Add(initialQuantity);

        // Act
        var updatedQuantity = _sensorService.SetQuantity(id, 20, "Fahrenheit");

        // Assert
        Assert.Equal(20, updatedQuantity.Value);
        Assert.Equal("Fahrenheit", updatedQuantity.Unit);
    }

    [Fact]
    public void ReadQuantity_ShouldReturnCorrectQuantity()
    {
        // Arrange
        var id = "sensor1";
        var expectedQuantity = new PhysicalQuantity(id) { Value = 10, Unit = "Celsius" };
        _quantitiesRepository.Add(expectedQuantity);

        // Act
        var actualQuantity = _sensorService.ReadQuantity(id);

        // Assert
        Assert.Equal(expectedQuantity, actualQuantity);
    }

    [Fact]
    public void ReadParameter_ShouldReturnCalculatedParameter()
    {
        // Arrange
        var id = "sensor1";
        var config = new SensorConfig(id)
        {
            RandomErrorConfig = new RandomErrorConfig { Type = RandomErrorType.Gaussian, Mean = 0, StandardDeviation = 0 },
            StaticFunctionConfig = new StaticFunctionConfig { Type = StaticFunctionType.Polynomial, Coefficients = new List<double>() { 0.0, 1.0 } },
            SystematicErrorConfig = new SystematicErrorConfig { Type = SystematicErrorType.Constant, Value = 2 }
        };
        var quantity = new PhysicalQuantity(id) { Value = 10, Unit = "Celsius" };
        _sensorConfigsRepository.Add(config);
        _quantitiesRepository.Add(quantity);

        // Act
        var parameter = _sensorService.ReadParameter(id);

        // Assert
        Assert.Equal(12, parameter);
    }

    [Fact]
    public void ReadParameter_ShouldThrowException_WhenRandomErrorConfigIsMissing()
    {
        // Arrange
        var id = "sensor1";
        var config = new SensorConfig(id)
        {
            RandomErrorConfig = new RandomErrorConfig { Type = "", Mean = 0, StandardDeviation = 0 },
            StaticFunctionConfig = new StaticFunctionConfig { Type = StaticFunctionType.Polynomial, Coefficients = new List<double>() { 0.0, 1.0 } },
            SystematicErrorConfig = new SystematicErrorConfig { Type = SystematicErrorType.Constant, Value = 2 }
        };
        var quantity = new PhysicalQuantity(id) { Value = 10, Unit = "Celsius" };
        _sensorConfigsRepository.Add(config);
        _quantitiesRepository.Add(quantity);

        // Act
        Action act = () => _sensorService.ReadParameter(id);

        // Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(act);
        Assert.Contains("RandomErrorConfig", exception.Message);
    }
    
    [Fact]
    public void ReadParameter_ShouldThrowException_WhenStaticFunctionConfigIsMissing()
    {
        // Arrange
        var id = "sensor1";
        var config = new SensorConfig(id)
        {
            RandomErrorConfig = new RandomErrorConfig { Type = RandomErrorType.Gaussian, Mean = 0, StandardDeviation = 0 },
            StaticFunctionConfig = new StaticFunctionConfig { Type = "", Coefficients = new List<double>() { 0.0, 1.0 } },
            SystematicErrorConfig = new SystematicErrorConfig { Type = SystematicErrorType.Constant, Value = 2 }
        };
        var quantity = new PhysicalQuantity(id) { Value = 10, Unit = "Celsius" };
        _sensorConfigsRepository.Add(config);
        _quantitiesRepository.Add(quantity);

        // Act
        Action act = () => _sensorService.ReadParameter(id);

        // Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(act);
        Assert.Contains("StaticFunctionConfig", exception.Message);
    }
    
    [Fact]
    public void ReadParameter_ShouldThrowException_WhenSystematicErrorConfigIsMissing()
    {
        // Arrange
        var id = "sensor1";
        var config = new SensorConfig(id)
        {
            RandomErrorConfig = new RandomErrorConfig { Type = RandomErrorType.Gaussian, Mean = 0, StandardDeviation = 0 },
            StaticFunctionConfig = new StaticFunctionConfig { Type = StaticFunctionType.Polynomial, Coefficients = new List<double>() { 0.0, 1.0 } },
            SystematicErrorConfig = new SystematicErrorConfig { Type = "", Value = 2 }
        };
        var quantity = new PhysicalQuantity(id) { Value = 10, Unit = "Celsius" };
        _sensorConfigsRepository.Add(config);
        _quantitiesRepository.Add(quantity);

        // Act
        Action act = () => _sensorService.ReadParameter(id);

        // Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(act);
        Assert.Contains("SystematicErrorConfig", exception.Message);
    }

    [Fact]
    public void ReadLinearRegression_ShouldLogParameterAndQuantity()
    {
        // Arrange
        var id = "sensor1";
        var config = new SensorConfig(id)
        {
            RandomErrorConfig = new RandomErrorConfig { Type = RandomErrorType.Gaussian, Mean = 0, StandardDeviation = 1 },
            StaticFunctionConfig = new StaticFunctionConfig { Type = StaticFunctionType.Polynomial, Coefficients = new List<double>() { 1.0, 0.0 } },
            SystematicErrorConfig = new SystematicErrorConfig { Type = SystematicErrorType.Constant, Value = 2 }
        };
        var quantity = new PhysicalQuantity(id) { Value = 10, Unit = "Celsius" };
        _sensorConfigsRepository.Add(config);
        _quantitiesRepository.Add(quantity);

        // Act
        _sensorService.ReadLinearRegression(id);

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Parameter") && v.ToString().Contains("Quantity")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()
            ),
            Times.Once
        );
    }

    [Fact]
    public void GetConfig_ShouldReturnCorrectConfig()
    {
        // Arrange
        var id = "sensor1";
        var expectedConfig = new SensorConfig(id) { RandomErrorConfig = new RandomErrorConfig(), StaticFunctionConfig = new StaticFunctionConfig(), SystematicErrorConfig = new SystematicErrorConfig() };
        _sensorConfigsRepository.Add(expectedConfig);

        // Act
        var actualConfig = _sensorService.GetConfig(id);

        // Assert
        Assert.Equal(expectedConfig, actualConfig);
    }
    
    [Fact]
    public void Delete_ShouldRemoveSensorConfigAndQuantity()
    {
        // Arrange
        var id = "sensor1";
        var config = new SensorConfig(id);
        var quantity = new PhysicalQuantity(id);
        _sensorConfigsRepository.Add(config);
        _quantitiesRepository.Add(quantity);

        // Act
        _sensorService.Delete(id);

        // Assert
        Assert.Null(_sensorConfigsRepository.Get(id));
        Assert.Null(_quantitiesRepository.Get(id));
    }
}