using CriticalConditionBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace CriticalConditionBackend.Data
{
    public class CriticalConditionDbContext : DbContext
    {
        public CriticalConditionDbContext(DbContextOptions<CriticalConditionDbContext> dbContext) : base(dbContext)
        {
        }

        public DbSet<SuperUser> SuperUsers { get; set; }
        public DbSet<SubUser> SubUsers { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<EditsLog> EditsLogs { get; set; }
    }
}
