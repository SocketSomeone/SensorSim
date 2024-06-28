namespace SensorSim.Domain.Interface;

public interface ICalculable
{
    double Calculate(double value);
}

public interface IRandomError : ICalculable
{
}

public interface IStaticFunction : ICalculable
{
}

public interface ISystematicError : ICalculable
{
}

public interface IContinuousFunction : ICalculable
{
}

public interface IMotionFunction : ICalculable
{
    public void SetDestination(double destination);
    
    public void SetSpeed(double speed);
    
    public bool IsStable(double value);
}