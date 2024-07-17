using Microsoft.AspNetCore.Mvc;
using Moq;
using SensorSim.Actuator.API.Controllers;
using SensorSim.Actuator.API.Interface;
using SensorSim.Domain.DTO.Actuator;
using SensorSim.Domain.Model;

namespace SensorSim.Actuator.API.Tests;

public class ActuatorControllerTests
{
    private readonly Mock<IActuatorService> _actuatorServiceMock;
    private readonly ActuatorController _actuatorController;

    public ActuatorControllerTests()
    {
        _actuatorServiceMock = new Mock<IActuatorService>();
        _actuatorController = new ActuatorController(_actuatorServiceMock.Object);
    }

    [Fact]
    public void GetAll_ReturnsAllActuators()
    {
        // Arrange
        var actuators = new List<string> { "Actuator1", "Actuator2" };
        var currentQuantity = new PhysicalQuantity("") { Value = 10, Unit = "Unit1" };
        var targetQuantity = new PhysicalQuantity("") { Value = 20, Unit = "Unit2" };
        var exposures = new Queue<PhysicalExposure>();

        _actuatorServiceMock.Setup(s => s.GetActuators()).Returns(actuators.ToArray());
        _actuatorServiceMock.Setup(s => s.ReadCurrentQuantity(It.IsAny<string>())).Returns(currentQuantity);
        _actuatorServiceMock.Setup(s => s.ReadTargetQuantity(It.IsAny<string>())).Returns(targetQuantity);
        _actuatorServiceMock.Setup(s => s.ReadExposures(It.IsAny<string>())).Returns(exposures);

        // Act
        var result = _actuatorController.GetAll();
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var actuatorsResult = Assert.IsAssignableFrom<IEnumerable<GetActuatorResponseModel>>(okResult.Value);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, okResult.StatusCode);
        Assert.Equal(2, actuatorsResult.Count());
        Assert.All(actuatorsResult, a =>
        {
            Assert.Equal(currentQuantity, a.Current);
            Assert.Equal(targetQuantity, a.Target);
            Assert.Equal(currentQuantity.Value == targetQuantity.Value, a.IsOnTarget);
            Assert.Equal(exposures, a.Exposures);
            Assert.Empty(a.ExternalFactors);
        });
    }

    [Fact]
    public void Get_ReturnsActuatorById()
    {
        // Arrange
        var actuatorId = "Actuator1";
        var currentQuantity = new PhysicalQuantity(actuatorId) { Value = 10, Unit = "Unit1" };
        var targetQuantity = new PhysicalQuantity(actuatorId) { Value = 20, Unit = "Unit2" };
        var exposures = new Queue<PhysicalExposure>();

        _actuatorServiceMock.Setup(s => s.ReadCurrentQuantity(actuatorId)).Returns(currentQuantity);
        _actuatorServiceMock.Setup(s => s.ReadTargetQuantity(actuatorId)).Returns(targetQuantity);
        _actuatorServiceMock.Setup(s => s.ReadExposures(actuatorId)).Returns(exposures);

        // Act
        var result = _actuatorController.Get(actuatorId);
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<GetActuatorResponseModel>(okResult.Value);


        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, okResult.StatusCode);
        Assert.Equal(currentQuantity, response.Current);
        Assert.Equal(targetQuantity, response.Target);
        Assert.Equal(currentQuantity.Value == targetQuantity.Value, response.IsOnTarget);
        Assert.Equal(exposures, response.Exposures);
        Assert.Empty(response.ExternalFactors);
    }

    [Fact]
    public void Set_UpdatesActuator()
    {
        // Arrange
        var actuatorId = "Actuator1";
        var setActuatorModel = new SetActuatorRequestModel
        {
            CurrentQuantity = new TargetQuantityRequestModel { Value = 15, Unit = "Unit3" },
            TargetQuantity = new TargetQuantityRequestModel { Value = 25, Unit = "Unit4" },
            Exposures = new Queue<PhysicalExposure>()
        };
        var currentQuantity = new PhysicalQuantity(actuatorId) { Value = 15, Unit = "Unit3" };
        var targetQuantity = new PhysicalQuantity(actuatorId) { Value = 25, Unit = "Unit4" };
        var exposures = new Queue<PhysicalExposure>();

        _actuatorServiceMock.Setup(s => s.ReadCurrentQuantity(actuatorId)).Returns(currentQuantity);
        _actuatorServiceMock.Setup(s => s.ReadTargetQuantity(actuatorId)).Returns(targetQuantity);
        _actuatorServiceMock.Setup(s => s.ReadExposures(actuatorId)).Returns(exposures);

        // Act
        var result = _actuatorController.Set(actuatorId, setActuatorModel);
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<GetActuatorResponseModel>(okResult.Value);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, okResult.StatusCode);
        Assert.Equal(currentQuantity, response.Current);
        Assert.Equal(targetQuantity, response.Target);
        Assert.Equal(Math.Abs(currentQuantity.Value - targetQuantity.Value) < 0.1, response.IsOnTarget);
        Assert.Equal(exposures, response.Exposures);
        Assert.Empty(response.ExternalFactors);

        _actuatorServiceMock.Verify(
            s => s.SetCurrentQuantity(actuatorId, setActuatorModel.CurrentQuantity.Value,
                setActuatorModel.CurrentQuantity.Unit), Times.Once);
        _actuatorServiceMock.Verify(
            s => s.SetTargetQuantity(actuatorId, setActuatorModel.TargetQuantity.Value,
                setActuatorModel.TargetQuantity.Unit), Times.Once);
        _actuatorServiceMock.Verify(s => s.SetExposures(actuatorId, It.IsAny<Queue<PhysicalExposure>>()), Times.Once);
    }
    
    [Fact]
    public void Set_UpdatesActuatorWithoutCurrentQuantity()
    {
        // Arrange
        var actuatorId = "Actuator1";
        var setActuatorModel = new SetActuatorRequestModel
        {
            TargetQuantity = new TargetQuantityRequestModel { Value = 25, Unit = "Unit4" },
            Exposures = new Queue<PhysicalExposure>()
        };
        var currentQuantity = new PhysicalQuantity(actuatorId) { Value = 15, Unit = "Unit3" };
        var targetQuantity = new PhysicalQuantity(actuatorId) { Value = 25, Unit = "Unit4" };
        var exposures = new Queue<PhysicalExposure>();

        _actuatorServiceMock.Setup(s => s.ReadCurrentQuantity(actuatorId)).Returns(currentQuantity);
        _actuatorServiceMock.Setup(s => s.ReadTargetQuantity(actuatorId)).Returns(targetQuantity);
        _actuatorServiceMock.Setup(s => s.ReadExposures(actuatorId)).Returns(exposures);

        // Act
        var result = _actuatorController.Set(actuatorId, setActuatorModel);
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<GetActuatorResponseModel>(okResult.Value);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, okResult.StatusCode);
        Assert.Equal(currentQuantity, response.Current);
        Assert.Equal(targetQuantity, response.Target);
        Assert.Equal(Math.Abs(currentQuantity.Value - targetQuantity.Value) < 0.1, response.IsOnTarget);
        Assert.Equal(exposures, response.Exposures);
        Assert.Empty(response.ExternalFactors);

        _actuatorServiceMock.Verify(
            s => s.SetCurrentQuantity(actuatorId, currentQuantity.Value, setActuatorModel.TargetQuantity.Unit), Times.Once);
        _actuatorServiceMock.Verify(
            s => s.SetTargetQuantity(actuatorId, setActuatorModel.TargetQuantity.Value,
                setActuatorModel.TargetQuantity.Unit), Times.Once);
        _actuatorServiceMock.Verify(s => s.SetExposures(actuatorId, It.IsAny<Queue<PhysicalExposure>>()), Times.Once);
    }

    [Fact]
    public void Read_Events()
    {
        // Arrange
        var actuatorId = "Actuator1";
        var events = new List<ActuatorEvent>
        {
            new ActuatorEvent($"{actuatorId}:1") { ActuatorId = actuatorId, Name = "Event1", Value = 1, },
            new ActuatorEvent($"{actuatorId}:2") { ActuatorId = actuatorId, Name = "Event2", Value = 2 }
        };

        _actuatorServiceMock.Setup(s => s.GetEvents(actuatorId)).Returns(events);


        // Act
        var result = _actuatorController.GetEvents(actuatorId);
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsAssignableFrom<IEnumerable<ActuatorEvent>>(okResult.Value);


        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, okResult.StatusCode);
        Assert.Equal(events.Count, response.Count());
        Assert.All(response, e =>
        {
            Assert.Equal(actuatorId, e.ActuatorId);
            Assert.StartsWith(actuatorId, e.Id);
            Assert.Contains("Event", e.Name);
            Assert.IsType<int>(e.Value);
        });
    }
    
    [Fact]
    public void Delete_RemovesActuator()
    {
        // Arrange
        var actuatorId = "Actuator1";

        // Act
        var result = _actuatorController.Delete(actuatorId);
        var okResult = Assert.IsType<OkResult>(result);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, okResult.StatusCode);
        _actuatorServiceMock.Verify(s => s.Delete(actuatorId), Times.Once);
    }
}