using prueba.bounded_context.Domain.Model.Aggregates;
using prueba.bounded_context.Domain.Model.Queries;

namespace prueba.bounded_context.Domain.Services;

/// <summary>
///     Query service interface for Bill aggregate
/// </summary>
public interface IBillQueryService
{
    /// <summary>
    ///     Handle get bill by id query
    /// </summary>
    Task<Bill?> Handle(GetBillByIdQuery query);
}
