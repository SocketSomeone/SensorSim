using SensorSim.Infrastructure.Helpers;

namespace SensorSim.Infrastructure.Tests.Helpers;

public class PolynomialStaticFunctionTests
{
    [Fact]
    public void Calculate_ReturnsCorrectValue_ForSingleCoefficient()
    {
        // Arrange
        var coefficients = new List<double> { 5 }; // 5
        var polynomial = new PolynomialStaticFunction(coefficients);

        // Act
        var result = polynomial.Calculate(2);

        // Assert
        Assert.Equal(5, result);
    }

    [Fact]
    public void Calculate_ReturnsCorrectValue_ForLinearFunction()
    {
        // Arrange
        var coefficients = new List<double> { 2, 3 }; // 2 + 3x
        var polynomial = new PolynomialStaticFunction(coefficients);

        // Act
        var result = polynomial.Calculate(2);

        // Assert
        Assert.Equal(8, result); // 2 + 3*2 = 8
    }

    [Fact]
    public void Calculate_ReturnsCorrectValue_ForQuadraticFunction()
    {
        // Arrange
        var coefficients = new List<double> { 1, 0, 4 }; // 1 + 0*x + 4*x^2
        var polynomial = new PolynomialStaticFunction(coefficients);

        // Act
        var result = polynomial.Calculate(2);

        // Assert
        Assert.Equal(17, result); // 1 + 0*2 + 4*2^2 = 1 + 16 = 17
    }

    [Fact]
    public void Calculate_ReturnsCorrectValue_ForCubicFunction()
    {
        // Arrange
        var coefficients = new List<double> { 1, -1, 2, 3 }; // 1 - x + 2x^2 + 3x^3
        var polynomial = new PolynomialStaticFunction(coefficients);

        // Act
        var result = polynomial.Calculate(2);

        // Assert
        Assert.Equal(31, result); // 1 - 2 + 2*4 + 3*8 = 1 - 2 + 8 + 24 = 31
    }
}