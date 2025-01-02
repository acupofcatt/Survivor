using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Survivor.Entities;

namespace Survivor.Data;

public class SurvivorContext : DbContext
{
    public SurvivorContext(DbContextOptions<SurvivorContext> options) : base(options)
    {
    }

    public DbSet<CompetitorEntity> Competitors { get; set; }

    public DbSet<CategoryEntity> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CompetitorEntity>()
            .HasOne(c => c.Category);

        modelBuilder.Entity<CategoryEntity>()
            .HasMany(c => c.Competitors);

        modelBuilder.Entity<CategoryEntity>()
            .HasData(new CategoryEntity
                {
                    Id = 1, Name = "Ünlüler", CreatedDate = DateTimeOffset.Now, ModifiedDate = DateTimeOffset.Now, IsDeleted = false
                },
                new CategoryEntity
                {
                    Id = 2, Name = "Gönüllüler", CreatedDate = DateTimeOffset.Now, ModifiedDate = DateTimeOffset.Now,
                    IsDeleted = false
                }
            );
        
    }
}