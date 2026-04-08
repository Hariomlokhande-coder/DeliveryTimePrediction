using Microsoft.EntityFrameworkCore;
using DeliveryTimePrediction.Models;

namespace DeliveryTimePrediction.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {}
        public DbSet<User> Users { get; set; }
    }
}