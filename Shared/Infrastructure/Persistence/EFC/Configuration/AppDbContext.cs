
using prueba.Shared.Infrastructure.Persistence.EFC.Configuration.Extensions;
using prueba.bounded_context.Domain.Model.Aggregates;
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
        
        // Bill entity configuration
        builder.Entity<Bill>(entity =>
        {
            entity.HasKey(b => b.BillNumber);
            entity.Property(b => b.BillNumber)
                .IsRequired()
                .ValueGeneratedOnAdd();
            
            entity.Property(b => b.Customer)
                .IsRequired()
                .HasMaxLength(100);
            
            entity.Property(b => b.ServiceId)
                .IsRequired();
            
            entity.Property(b => b.Plate)
                .HasMaxLength(10);
            
            entity.Property(b => b.Emission)
                .IsRequired();
            
            entity.OwnsOne(b => b.Invoice, invoice =>
            {
                invoice.Property(i => i.SerialNumber)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("invoice_serial_number");
                
                invoice.Property(i => i.SequentialNumber)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("invoice_sequential_number");
            });
            
            entity.Property(b => b.Amount)
                .IsRequired();
            
            entity.Property(b => b.Adviser)
                .IsRequired()
                .HasMaxLength(100);
        });

        builder.UseSnakeCaseNamingConvention();
    }
}