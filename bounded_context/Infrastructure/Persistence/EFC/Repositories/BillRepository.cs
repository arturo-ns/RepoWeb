using prueba.bounded_context.Domain.Model.Aggregates;
using prueba.bounded_context.Domain.Repositories;
using prueba.Shared.Infrastructure.Persistence.EFC.Configuration;
using prueba.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace prueba.bounded_context.Infrastructure.Persistence.EFC.Repositories;

/// <summary>
///     Repository implementation for Bill aggregate
/// </summary>
public class BillRepository(AppDbContext context) : BaseRepository<Bill>(context), IBillRepository
{
    public async Task<Bill?> FindByInvoiceAsync(string serialNumber, string sequentialNumber)
    {
        return await Context.Set<Bill>()
            .FirstOrDefaultAsync(b => 
                b.Invoice.SerialNumber == serialNumber && 
                b.Invoice.SequentialNumber == sequentialNumber);
    }
}
