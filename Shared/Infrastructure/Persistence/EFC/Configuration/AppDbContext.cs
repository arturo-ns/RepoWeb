
using prueba.Shared.Infrastructure.Persistence.EFC.Configuration.Extensions;
using EntityFrameworkCore.CreatedUpdatedDate.Extensions;
using Microsoft.EntityFrameworkCore;

namespace prueba.Shared.Infrastructure.Persistence.EFC.Configuration;

/// <summary>
///     Application database context
/// </summary>
public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        // Add the created and updated interceptor
        builder.AddCreatedUpdatedInterceptor();
        base.OnConfiguring(builder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        // Modificar y Agregar tus entidades:
        //builder.Entity<TuEntidad>().HasKey(f => f.Id);
        //builder.Entity<TuEntidad>().Property(f => f.Id).IsRequired().ValueGeneratedOnAdd();
        //builder.Entity<TuEntidad>().Property(f => f.SourceId).IsRequired();
        //builder.Entity<TuEntidad>().Property(f => f.NewsApiKey).IsRequired();

        builder.UseSnakeCaseNamingConvention();
    }
}