using prueba.bounded_context.Domain.Model.Aggregates;
using prueba.bounded_context.Domain.Model.Queries;
using prueba.bounded_context.Domain.Repositories;
using prueba.bounded_context.Domain.Services;

namespace prueba.bounded_context.Application.Internal.QueryServices;

/// <summary>
///     Query service implementation for Bill aggregate
/// </summary>
public class BillQueryService(IBillRepository repository) : IBillQueryService
{
    public async Task<Bill?> Handle(GetBillByIdQuery query)
    {
        return await repository.FindByIdAsync(query.BillNumber);
    }
}
