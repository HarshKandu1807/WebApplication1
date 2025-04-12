using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Model
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions options):base(options)
        {
            
        }
        public DbSet<Users> Users { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
