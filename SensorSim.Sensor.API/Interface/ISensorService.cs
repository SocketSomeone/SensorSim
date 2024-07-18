using SensorSim.Domain.Model;

namespace SensorSim.Sensor.API.Interface;

public interface ISensorService
{
    PhysicalQuantity SetQuantity(string id, double value, string unit);
    
    PhysicalQuantity ReadQuantity(string id);
    
    double ReadParameter(string id);

    double ReadApproximatedValue(string id, double parameter);
    
    SensorConfig GetConfig(string sensorId);
    
    string[] GetSensors();
    
    void Delete(string sensorId);
}