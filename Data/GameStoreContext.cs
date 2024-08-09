using GameStore.api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;

namespace GameStore.api.Data;


public class GameStoreContext(DbContextOptions<GameStoreContext> options) 
    : DbContext(options)
{

    public DbSet<Game> Games => Set<Game>();
    public DbSet<Genre> Genres => Set<Genre>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Genre>().HasData(
            new {Id = 1, Name = "Sandbox"}, 
            new {Id = 2, Name = "Shooter"}, 
            new {Id = 3, Name = "SoulsLike"}, 
            new {Id = 4, Name = "Roguelike"},
            new {Id = 5, Name = "RPG"} 
        );
    }

}