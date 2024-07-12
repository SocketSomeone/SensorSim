using SensorSim.Infrastructure.Helpers;

namespace SensorSim.Infrastructure.Tests.Helpers;

    public class GaussianRandomErrorTests
    {
        [Fact]
        public void Calculate_ShouldReturnValueWithinExpectedRange()
        {
            // Arrange
            double mean = 0.0;
            double standardDeviation = 1.0;
            var gaussianRandomError = new GaussianRandomError(mean, standardDeviation);
            double value = 0.0;
            int sampleSize = 10000;
            double sum = 0.0;

            // Act
            for (int i = 0; i < sampleSize; i++)
            {
                sum += gaussianRandomError.Calculate(value);
            }
            double average = sum / sampleSize;

            // Assert
            // Mean should be close to the specified mean (0.0 in this case)
            Assert.InRange(average, mean - 0.1, mean + 0.1);
        }

        [Fact]
        public void Calculate_ShouldReturnValuesWithExpectedStandardDeviation()
        {
            // Arrange
            double mean = 0.0;
            double standardDeviation = 1.0;
            var gaussianRandomError = new GaussianRandomError(mean, standardDeviation);
            double value = 0.0;
            int sampleSize = 10000;
            double[] values = new double[sampleSize];

            // Act
            for (int i = 0; i < sampleSize; i++)
            {
                values[i] = gaussianRandomError.Calculate(value);
            }
            double calculatedStdDev = CalculateStandardDeviation(values);

            // Assert
            // Standard deviation should be close to the specified standard deviation (1.0 in this case)
            Assert.InRange(calculatedStdDev, standardDeviation - 0.1, standardDeviation + 0.1);
        }

        private double CalculateStandardDeviation(double[] values)
        {
            double mean = values.Average();
            double sumOfSquaresOfDifferences = values.Select(val => (val - mean) * (val - mean)).Sum();
            return Math.Sqrt(sumOfSquaresOfDifferences / values.Length);
        }

        [Fact]
        public void Calculate_ShouldReturnDifferentValues()
        {
            // Arrange
            double mean = 0.0;
            double standardDeviation = 1.0;
            var gaussianRandomError = new GaussianRandomError(mean, standardDeviation);
            double value = 0.0;

            // Act
            double result1 = gaussianRandomError.Calculate(value);
            double result2 = gaussianRandomError.Calculate(value);

            // Assert
            Assert.NotEqual(result1, result2);
        }
    }