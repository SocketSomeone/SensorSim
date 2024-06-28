using SensorSim.Domain;
using SensorSim.Domain.Interface;

namespace SensorSim.API.Config;

public interface ISensorConfig<T> where T : IPhysicalQuantity
{
   T DefaultValue { get; }
   
   IStaticFunction StaticFunction { get; }
   
   ISystematicError SystematicError { get; }
   
   IRandomError RandomError { get; }
   
   double Inertia { get; }
   
   Queue<PhysicalValueExposure> Exposures { get; }
}