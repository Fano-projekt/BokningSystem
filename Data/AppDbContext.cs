using Microsoft.EntityFrameworkCore;
using BokningSystem.Models; 

namespace BokningSystem.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<LedigaTider> LedigaTider { get; set; }

    }
}
