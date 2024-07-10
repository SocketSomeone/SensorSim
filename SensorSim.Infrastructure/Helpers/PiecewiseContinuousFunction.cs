using System.Drawing;
using SensorSim.Domain.Interface;

namespace SensorSim.Infrastructure.Helpers;

public class PiecewiseContinuousFunction : IContinuousFunction
{
    private readonly List<Point> _points;

    public PiecewiseContinuousFunction(List<Point> points)
    {
        _points = points;
    }

    public double Calculate(double value)
    {
        if (_points.Count == 0)
        {
            throw new InvalidOperationException("No points to calculate the piecewise function");
        }

        if (_points.Count == 1)
        {
            return _points[0].Y;
        }

        Point previousPoint = _points[0];
        for (var i = 1; i < _points.Count; i++)
        {
            var point = _points[i];
            if (value <= point.X)
            {
                var slope = (point.Y - previousPoint.Y) / (point.X - previousPoint.X);
                return previousPoint.Y + slope * (value - previousPoint.X);
            }

            previousPoint = point;
        }

        return previousPoint.Y;
    }
}