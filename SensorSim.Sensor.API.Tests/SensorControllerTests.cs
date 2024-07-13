using Microsoft.AspNetCore.Mvc;
using Moq;
using SensorSim.Domain.DTO.Sensor;
using SensorSim.Domain.Enums;
using SensorSim.Domain.Model;
using SensorSim.Sensor.API.Controllers;
using SensorSim.Sensor.API.Services;

namespace SensorSim.Sensor.API.Tests;

public class SensorControllerTests
{
    private readonly SensorController _controller;
    private readonly Mock<ISensorService> _mockSensorService;

    public SensorControllerTests()
    {
        _mockSensorService = new Mock<ISensorService>();
        _controller = new SensorController(_mockSensorService.Object);
    }

    [Fact]
    public void Get_ShouldReturnAllSensors()
    {
        // Arrange
        var sensorIds = new[] { "sensor1", "sensor2" };
        _mockSensorService.Setup(service => service.GetSensors()).Returns(sensorIds);
        _mockSensorService.Setup(service => service.ReadQuantity(It.IsAny<string>())).Returns((string id) =>
            new PhysicalQuantity(id) { Value = 10, Unit = "Celsius" });
        _mockSensorService.Setup(service => service.ReadParameter(It.IsAny<string>())).Returns(20);

        // Act
        var result = _controller.Get();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsAssignableFrom<IEnumerable<GetSensorResponseModel>>(okResult.Value);
        Assert.Equal(sensorIds.Length, response.Count());
    }

    [Fact]
    public void Get_ById_ShouldReturnSensor()
    {
        // Arrange
        var sensorId = "sensor1";
        _mockSensorService.Setup(service => service.ReadQuantity(sensorId)).Returns(new PhysicalQuantity(sensorId)
            { Value = 10, Unit = "Celsius" });
        _mockSensorService.Setup(service => service.ReadParameter(sensorId)).Returns(20);

        // Act
        var result = _controller.Get(sensorId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<GetSensorResponseModel>(okResult.Value);
        Assert.Equal(sensorId, response.Current.Id);
        Assert.Equal(10, response.Current.Value);
        Assert.Equal("Celsius", response.Current.Unit);
        Assert.Equal(20, response.Parameter);
    }

    [Fact]
    public void SetSensorValue_ShouldUpdateAndReturnSensor()
    {
        // Arrange
        var sensorId = "sensor1";
        var dto = new SetSensorValueRequestModel { Value = 20, Unit = "Fahrenheit"};
        _mockSensorService.Setup(service => service.SetQuantity(sensorId, dto.Value, dto.Unit))
            .Returns(new PhysicalQuantity(sensorId) { Value = dto.Value, Unit = dto.Unit });
        _mockSensorService.Setup(service => service.ReadParameter(sensorId)).Returns(30);

        // Act
        var result = _controller.SetSensorValue(sensorId, dto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<SetSensorResponseModel>(okResult.Value);
        Assert.Equal(sensorId, response.Current.Id);
        Assert.Equal(20, response.Current.Value);
        Assert.Equal("Fahrenheit", response.Current.Unit);
        Assert.Equal(30, response.Parameter);
    }

    [Fact]
    public void GetConfig_ShouldReturnSensorConfig()
    {
        // Arrange
        var sensorId = "sensor1";
        var expectedConfig = new SensorConfig(sensorId)
        {
            RandomErrorConfig = new RandomErrorConfig
                { Type = RandomErrorType.Gaussian, Mean = 0, StandardDeviation = 1 },
            StaticFunctionConfig = new StaticFunctionConfig
                { Type = StaticFunctionType.Polynomial, Coefficients = new List<double>() { 1.0, 0.0 } },
            SystematicErrorConfig = new SystematicErrorConfig { Type = SystematicErrorType.Constant, Value = 2 }
        };
        _mockSensorService.Setup(service => service.GetConfig(sensorId)).Returns(expectedConfig);

        // Act
        var result = _controller.GetConfig(sensorId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<SensorConfig>(okResult.Value);
        Assert.Equal(expectedConfig, response);
    }

    [Fact]
    public void SetConfig_ShouldUpdateAndReturnSensorConfig()
    {
        // Arrange
        var sensorId = "sensor1";
        var setConfig = new SetSensorConfigRequestModel
        {
            StaticFunctionConfig = new StaticFunctionConfig
            {
                Coefficients = new List<double> { 0, 1, 2, 3 }
            }
        };
        var existingConfig = new SensorConfig(sensorId)
        {
            StaticFunctionConfig = new StaticFunctionConfig { Coefficients = new List<double>() { 1.0, 2.0 } }
        };
        _mockSensorService.Setup(service => service.GetConfig(sensorId)).Returns(existingConfig);

        // Act
        var result = _controller.SetConfig(sensorId, setConfig);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<SensorConfig>(okResult.Value);
        Assert.Equal(new List<double> { 2, 3 }, response.StaticFunctionConfig.Coefficients);
    }
}