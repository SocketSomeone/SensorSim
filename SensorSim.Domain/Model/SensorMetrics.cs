namespace SensorSim.Domain;

public class SensorMetrics
{
    public double MinValue { get; set; }

    public double MaxValue { get; set; }

    public double AverageValue => (MinValue + MaxValue) / 2;

    public double Range => MaxValue - MinValue;

    public SensorMetrics()
    {
        MinValue = double.MaxValue;
        MaxValue = double.MinValue;
    }

    public SensorMetrics(double minValue, double maxValue)
    {
        MinValue = minValue;
        MaxValue = maxValue;
    }


    public void Update(double value)
    {
        if (value < MinValue)
        {
            MinValue = value;
        }

        if (value > MaxValue)
        {
            MaxValue = value;
        }
    }
}