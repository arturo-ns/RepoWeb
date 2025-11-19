using prueba.bounded_context.Domain.Model.Aggregates;
using prueba.Shared.Domain.Repositories;

namespace prueba.bounded_context.Domain.Repositories;

/// <summary>
///     Repository interface for Bill aggregate
/// </summary>
public interface IBillRepository : IBaseRepository<Bill>
{
    /// <summary>
    ///     Find a bill by invoice serial number and sequential number
    /// </summary>
    Task<Bill?> FindByInvoiceAsync(string serialNumber, string sequentialNumber);
}
