using SensorSim.Infrastructure.Helpers;

namespace SensorSim.Infrastructure.Tests.Helpers;

public class ConstantSystematicErrorTests
{
    [Fact]
    public void Calculate_ReturnsConstantValue()
    {
        // Arrange
        double expectedValue = 5.0;
        var constantError = new ConstantSystematicError(expectedValue);

        // Act
        double result = constantError.Calculate(10.0); // аргумент не важен, так как метод всегда возвращает константу

        // Assert
        Assert.Equal(expectedValue, result);
    }

    [Fact]
    public void Calculate_ReturnsSameValueForDifferentInputs()
    {
        // Arrange
        double expectedValue = 3.14;
        var constantError = new ConstantSystematicError(expectedValue);

        // Act
        double result1 = constantError.Calculate(1.0);
        double result2 = constantError.Calculate(100.0);
        double result3 = constantError.Calculate(-50.0);

        // Assert
        Assert.Equal(expectedValue, result1);
        Assert.Equal(expectedValue, result2);
        Assert.Equal(expectedValue, result3);
    }
}