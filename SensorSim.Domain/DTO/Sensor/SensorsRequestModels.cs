using SensorSim.Domain.Model;

namespace SensorSim.Domain.DTO.Sensor;

public class SetSensorValueRequestModel
{
    public double Value { get; set; }

    public string Unit { get; set; }
}

public class SetSensorConfigRequestModel
{
    public required List<double> StaticFunctionCoefficients { get; set; }
    
    public required List<double> ApproximateCoefficients { get; set; }
}