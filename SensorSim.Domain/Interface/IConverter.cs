namespace SensorSim.Domain.Interface;

public interface IConverter
{
    IStaticFunction StaticFunction { get; set; }
    
    ISystematicError SystematicError { get; set; }
    
    IRandomError RandomError { get; set; }
    
    double Calculate(double value);
}