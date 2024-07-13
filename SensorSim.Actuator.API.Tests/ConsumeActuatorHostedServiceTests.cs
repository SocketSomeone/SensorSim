using Microsoft.Extensions.Logging;
using Moq;
using SensorSim.Actuator.API.Clients;
using SensorSim.Actuator.API.Services;

namespace SensorSim.Actuator.API.Tests;

    public class ConsumeActuatorHostedServiceTests
    {
        private readonly Mock<IServiceProvider> _serviceProviderMock;
        private readonly Mock<IActuatorService> _actuatorServiceMock;
        private readonly Mock<ILogger<ConsumeActuatorHostedService>> _loggerMock;
        private readonly ConsumeActuatorHostedService _consumeActuatorHostedService;

        public ConsumeActuatorHostedServiceTests()
        {
            _serviceProviderMock = new Mock<IServiceProvider>();
            _actuatorServiceMock = new Mock<IActuatorService>();
            _loggerMock = new Mock<ILogger<ConsumeActuatorHostedService>>();

            _serviceProviderMock
                .Setup(sp => sp.GetService(typeof(IActuatorService)))
                .Returns(_actuatorServiceMock.Object);

            _consumeActuatorHostedService = new ConsumeActuatorHostedService(_serviceProviderMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task ExecuteAsync_LogsInformationAndCallsDoWork()
        {
            // Arrange
            var cancellationToken = new CancellationTokenSource();

            // Act
            await _consumeActuatorHostedService.StartAsync(cancellationToken.Token);
            await _consumeActuatorHostedService.StopAsync(cancellationToken.Token);

            // Assert
            _loggerMock.Verify(
                logger => logger.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Consume Scoped Service Hosted Service running.")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
                Times.Once);

            _loggerMock.Verify(
                logger => logger.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Consume Scoped Service Hosted Service is working.")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
                Times.Once);
            
            _actuatorServiceMock.Verify(service => service.Update(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task StopAsync_LogsInformation()
        {
            // Arrange
            var cancellationToken = new CancellationToken(false);

            // Act
            await _consumeActuatorHostedService.StopAsync(cancellationToken);

            // Assert
            _loggerMock.Verify(
                logger => logger.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Consume Scoped Service Hosted Service is stopping.")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }
    }