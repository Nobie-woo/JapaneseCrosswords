using Microsoft.EntityFrameworkCore;
using AuthApi.Models;
namespace AuthApi.Data
{
public class AppDbContext : DbContext
{
public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
public DbSet<User> Users { get; set; } = null!;
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
 base.OnModelCreating(modelBuilder);
 // доп настройки сюда!!
}
}
}