using SensorSim.Domain.Model;

namespace SensorSim.Sensor.API.Interface;

public interface ISensorService
{
    PhysicalQuantity SetQuantity(string id, double value, string unit);
    
    PhysicalQuantity ReadQuantity(string id);
    
    double ReadParameter(string id);

    double ReadLinearRegression(string id);
    
    SensorConfig GetConfig(string sensorId);
    
    string[] GetSensors();
    
    void Delete(string sensorId);
}