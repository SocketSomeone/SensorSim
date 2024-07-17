using Microsoft.Extensions.Logging;
using Moq;
using SensorSim.Actuator.API.Interface;
using SensorSim.Actuator.API.Services;
using SensorSim.Domain.DTO.Sensor;
using SensorSim.Domain.Model;
using SensorSim.Infrastructure.Repositories;

namespace SensorSim.Actuator.API.Tests;

public class ActuatorServiceTests
{
    private readonly Mock<ILogger<ActuatorService>> _loggerMock;
    private readonly CrudMemoryRepository<ActuatorConfig> _actuatorConfigsRepository;
    private readonly CrudMemoryRepository<PhysicalQuantity> _quantitiesRepository;
    private readonly CrudMemoryRepository<ActuatorEvent> _actuatorEventsRepository;
    private readonly Mock<ISensorApi> _sensorApiMock;
    private readonly ActuatorService _actuatorService;

    public ActuatorServiceTests()
    {
        _loggerMock = new Mock<ILogger<ActuatorService>>();
        _actuatorConfigsRepository = new ActuatorConfigsRepository();
        _quantitiesRepository = new QuantitiesRepository();
        _actuatorEventsRepository = new ActuatorEventsRepository();
        _sensorApiMock = new Mock<ISensorApi>();
        _actuatorService = new ActuatorService(
            _loggerMock.Object,
            _actuatorConfigsRepository,
            _quantitiesRepository,
            _actuatorEventsRepository,
            _sensorApiMock.Object);
    }

    [Fact]
    public void GetActuators_ReturnsActuatorIds()
    {
        // Arrange
        var actuatorConfigs = new List<ActuatorConfig>
        {
            new("Actuator1"),
            new("Actuator2")
        };

        foreach (var config in actuatorConfigs.ToList())
        {
            _actuatorConfigsRepository.Add(config);
        }

        // Act
        var result = _actuatorService.GetActuators();

        // Assert
        Assert.Equal(new[] { "Actuator1", "Actuator2" }, result);
    }

    [Fact]
    public void SetCurrentQuantity_UpdatesQuantity()
    {
        // Arrange
        var actuatorId = "Actuator1";
        var quantity = new PhysicalQuantity(actuatorId) { Value = 0, Unit = "Unit1" };

        _quantitiesRepository.Add(quantity);

        // Act
        _actuatorService.SetCurrentQuantity(actuatorId, 10, "Unit2");

        // Assert
        Assert.Equal(10, quantity.Value);
        Assert.Equal("Unit2", quantity.Unit);
    }

    [Fact]
    public void SetTargetQuantity_UpdatesTargetQuantity()
    {
        // Arrange
        var actuatorId = "Actuator1";
        var config = new ActuatorConfig(actuatorId)
            { TargetQuantity = new PhysicalQuantity(actuatorId) { Value = 0, Unit = "Unit1" } };
        _actuatorConfigsRepository.Add(config);

        // Act
        _actuatorService.SetTargetQuantity(actuatorId, 20, "Unit3");

        // Assert
        Assert.Equal(20, config.TargetQuantity.Value);
        Assert.Equal("Unit3", config.TargetQuantity.Unit);
    }

    [Fact]
    public void SetExposures_UpdatesExposures()
    {
        // Arrange
        var actuatorId = "Actuator1";
        var exposures = new Queue<PhysicalExposure>();
        var config = new ActuatorConfig(actuatorId) { Exposures = new Queue<PhysicalExposure>() };
        _actuatorConfigsRepository.Add(config);

        // Act
        _actuatorService.SetExposures(actuatorId, exposures);

        // Assert
        Assert.Equal(exposures, config.Exposures);
    }

    [Fact]
    public void ReadCurrentQuantity_ReturnsQuantity()
    {
        // Arrange
        var actuatorId = "Actuator1";
        var quantity = new PhysicalQuantity(actuatorId) { Value = 10, Unit = "Unit1" };
        _quantitiesRepository.Add(quantity);

        // Act
        var result = _actuatorService.ReadCurrentQuantity(actuatorId);

        // Assert
        Assert.Equal(quantity, result);
    }

    [Fact]
    public void ReadTargetQuantity_ReturnsTargetQuantity()
    {
        // Arrange
        var actuatorId = "Actuator1";
        var config = new ActuatorConfig(actuatorId)
            { TargetQuantity = new PhysicalQuantity(actuatorId) { Value = 20, Unit = "Unit2" } };
        _actuatorConfigsRepository.Add(config);

        // Act
        var result = _actuatorService.ReadTargetQuantity(actuatorId);

        // Assert
        Assert.Equal(config.TargetQuantity, result);
    }

    [Fact]
    public void ReadExposures_ReturnsExposures()
    {
        // Arrange
        var actuatorId = "Actuator1";
        var exposures = new Queue<PhysicalExposure>();
        var config = new ActuatorConfig(actuatorId) { Exposures = exposures };
        _actuatorConfigsRepository.Add(config);

        // Act
        var result = _actuatorService.ReadExposures(actuatorId);

        // Assert
        Assert.Equal(exposures, result);
    }

    [Fact]
    public async Task Update_HandlesExposuresCorrectly()
    {
        // Arrange
        var stoppingToken = new CancellationTokenSource();
        var actuatorId = "Actuator1";
        var exposure = new PhysicalExposure { Value = 10, Speed = 1, Duration = 5 };
        var exposures = new Queue<PhysicalExposure>(new[] { exposure });
        var quantity = new PhysicalQuantity(actuatorId) { Value = 9, Unit = "Unit1" };
        var config = new ActuatorConfig(actuatorId) { Exposures = exposures, WaitUntil = DateTime.Now.AddSeconds(-1) };
        _actuatorConfigsRepository.Add(config);
        _quantitiesRepository.Add(quantity);

        _sensorApiMock.Setup(api => api.SetQuantity(It.IsAny<string>(), It.IsAny<SetSensorValueRequestModel>()))
            .ReturnsAsync(
                new SetSensorResponseModel()
            );
        _sensorApiMock.Setup(api => api.ReadQuantity(It.IsAny<string>()))
            .ReturnsAsync(new GetSensorResponseModel() { Current = new PhysicalQuantity(actuatorId), Parameter = 1.0 });

        // Act
        var updateTask = _actuatorService.Update(stoppingToken.Token);

        _actuatorService.ValueReachedExposureEvent += (sender, id, physicalExposure) =>
        {
            stoppingToken.Cancel();
            Assert.Empty(exposures);
        };

        Assert.Single(exposures);
        try
        {
            await updateTask;
        }
        catch (TaskCanceledException e)
        {
        }


        // Assert
        Assert.Empty(exposures);
        Assert.NotEmpty(_actuatorService.GetEvents(actuatorId));
        _sensorApiMock.Verify(api => api.SetQuantity(actuatorId, It.IsAny<SetSensorValueRequestModel>()),
            Times.AtLeastOnce);
    }

    [Fact]
    public async Task Update_HandlesWaitingCorrectly()
    {
        // Arrange
        var stoppingToken = new CancellationTokenSource();
        var actuatorId = "Actuator1";
        var exposure = new PhysicalExposure { Value = 10, Speed = 1, Duration = 5 };
        var exposures = new Queue<PhysicalExposure>(new[] { exposure });
        var quantity = new PhysicalQuantity(actuatorId) { Value = 9, Unit = "Unit1" };
        var config = new ActuatorConfig(actuatorId) { Exposures = exposures, WaitUntil = DateTime.Now.AddSeconds(10) };
        _actuatorConfigsRepository.Add(config);
        _quantitiesRepository.Add(quantity);

        // Act
        _actuatorService.Update(stoppingToken.Token);
        // Run and cancel after 1 second
        await Task.Delay(1000);
        stoppingToken.Cancel();

        // Assert
        Assert.Equal(9, quantity.Value);
        Assert.Single(exposures);
        _sensorApiMock.Verify(api => api.SetQuantity(actuatorId, It.IsAny<SetSensorValueRequestModel>()),
            Times.Never);
    }

    [Fact]
    public async Task Update_HandlesNoExposuresCorrectly()
    {
        // Arrange
        var stoppingToken = new CancellationTokenSource();
        var actuatorId = "Actuator1";
        var exposures = new Queue<PhysicalExposure>();
        var quantity = new PhysicalQuantity(actuatorId) { Value = 9, Unit = "Unit1" };
        var config = new ActuatorConfig(actuatorId) { Exposures = exposures, WaitUntil = DateTime.Now.AddSeconds(-1) };
        _actuatorConfigsRepository.Add(config);
        _quantitiesRepository.Add(quantity);


        // Act
        var updateTask = _actuatorService.Update(stoppingToken.Token);

        Assert.Empty(exposures);
        await Task.Delay(100);
        stoppingToken.Cancel();

        // Assert
        Assert.Empty(exposures);
        _sensorApiMock.Verify(api => api.SetQuantity(actuatorId, It.IsAny<SetSensorValueRequestModel>()),
            Times.Never);
        _sensorApiMock.Verify(api => api.ReadQuantity(actuatorId), Times.Never);
    }

    [Fact]
    public async Task Update_HandlesCancelledToken()
    {
        // Arrange
        var stoppingToken = new CancellationTokenSource();
        var actuatorId = "Actuator1";
        var exposure = new PhysicalExposure { Value = 10, Speed = 1, Duration = 5 };
        var exposures = new Queue<PhysicalExposure>(new[] { exposure });
        var quantity = new PhysicalQuantity(actuatorId) { Value = 9, Unit = "Unit1" };
        var config = new ActuatorConfig(actuatorId) { Exposures = exposures, WaitUntil = DateTime.Now.AddSeconds(-1) };
        _actuatorConfigsRepository.Add(config);
        _quantitiesRepository.Add(quantity);

        // Act
        stoppingToken.Cancel();
        await _actuatorService.Update(stoppingToken.Token);

        // Assert
        Assert.Equal(9, quantity.Value);
        Assert.Single(exposures);
        _sensorApiMock.Verify(api => api.SetQuantity(actuatorId, It.IsAny<SetSensorValueRequestModel>()),
            Times.Never);
        _sensorApiMock.Verify(api => api.ReadQuantity(actuatorId), Times.Never);
    }

    [Fact]
    public void Delete_ShouldRemoveActuatorConfigAndQuantity()
    {
        // Arrange
        var actuatorId = "Actuator1";
        var config = new ActuatorConfig(actuatorId);
        var quantity = new PhysicalQuantity(actuatorId);
        var actuatorEvent = new ActuatorEvent("1") { ActuatorId = actuatorId };
        _actuatorConfigsRepository.Add(config);
        _quantitiesRepository.Add(quantity);
        _actuatorEventsRepository.Add(actuatorEvent);

        // Act
        _actuatorService.Delete(actuatorId);

        // Assert
        Assert.Null(_actuatorConfigsRepository.Get(actuatorId));
        Assert.Null(_quantitiesRepository.Get(actuatorId));
        Assert.Empty(_actuatorService.GetEvents(actuatorId));
        _sensorApiMock.Verify(api => api.Delete(actuatorId), Times.Once);
    }
}