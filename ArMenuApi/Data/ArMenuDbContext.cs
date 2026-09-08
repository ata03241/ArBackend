using ArMenuApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ArMenuApi.Data;

public class ArMenuDbContext : DbContext
{
    public ArMenuDbContext(DbContextOptions<ArMenuDbContext> options) : base(options)
    {
    }

    public DbSet<Restaurant> Restaurants => Set<Restaurant>();
    public DbSet<Dish> Dishes => Set<Dish>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Restaurant>()
            .HasMany(r => r.Dishes)
            .WithOne(d => d.Restaurant!)
            .HasForeignKey(d => d.RestaurantId)
            .OnDelete(DeleteBehavior.Cascade);

        // resturant can`t have two dishes with the same targetIndex
        modelBuilder.Entity<Dish>()
            .HasIndex(d => new { d.RestaurantId, d.TargetIndex })
            .IsUnique();
    }
}