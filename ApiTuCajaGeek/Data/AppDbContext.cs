using Microsoft.EntityFrameworkCore;
using ApiTuCajaGeek.Models;

namespace ApiTuCajaGeek.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}
