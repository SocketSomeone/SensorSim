using Microsoft.EntityFrameworkCore;
using SensorSim.Infrastructure;

namespace SensorSim.Test.Fixtures;

public class DatabaseFixture : IDisposable
{
    public DataContext Context { get; }
    
    public DatabaseFixture()
    {
        
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseSqlServer("Server=localhost;User Id=sa;Password=P@ssw0rd1;Database=SensorSim;trustServerCertificate=true;MultipleActiveResultSets=true")
            .Options;
        
        Context = new DataContext(options);
        Context.Database.EnsureCreated();
    }
    
    public void Dispose()
    {
        Context.Database.EnsureDeleted();
    }
}