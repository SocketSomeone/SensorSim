using SensorSim.Infrastructure.Helpers;

namespace SensorSim.Infrastructure.Tests.Helpers;

public class InertiaMotionFunctionTests
{
    [Theory]
    [InlineData(0.1, 0, 1, 1, 0.1)]
    [InlineData(0.1, 1, 0, 1, 0.9)]
    [InlineData(0.1, 0, 0.05, 1, 0.05)]
    [InlineData(0.1, 0, 1, 0.5, 0.05)]
    [InlineData(0.1, 1, 0, 0.5, 0.95)]
    [InlineData(0.1, 1, 1, 1, 1)]
    [InlineData(0.1, 0, 0, 1, 0)]
    public void Calculate_WithGivenInputs_ReturnsExpectedResult(
        double rateOfChange, double value, double destination, double speed, double expected)
    {
        // Arrange
        var inertiaMotionFunction = new InertiaMotionFunction(rateOfChange);

        // Act
        var result = inertiaMotionFunction.Calculate(value, destination, speed);

        // Assert
        Assert.Equal(expected, result, 3); // Допустимое отклонение в 3 знака после запятой
    }

    [Theory]
    [InlineData(0.1, 0, 0)]
    [InlineData(0.1, 1, 1)]
    public void Calculate_WithDestinationEqualToValue_ReturnsValue(
        double rateOfChange, double value, double expected)
    {
        // Arrange
        var inertiaMotionFunction = new InertiaMotionFunction(rateOfChange);

        // Act
        var result = inertiaMotionFunction.Calculate(value);

        // Assert
        Assert.Equal(expected, result, 3); // Допустимое отклонение в 3 знака после запятой
    }
}