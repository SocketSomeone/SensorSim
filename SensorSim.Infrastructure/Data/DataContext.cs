using Microsoft.EntityFrameworkCore;
using SensorSim.Domain;

namespace SensorSim.Infrastructure;

public class DataContext : DbContext
{
    // public virtual DbSet<User> Users { get; set; }

    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }
}