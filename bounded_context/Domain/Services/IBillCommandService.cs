using prueba.bounded_context.Domain.Model.Aggregates;
using prueba.bounded_context.Domain.Model.Commands;

namespace prueba.bounded_context.Domain.Services;

/// <summary>
///     Command service interface for Bill aggregate
/// </summary>
public interface IBillCommandService
{
    /// <summary>
    ///     Handle create bill command
    /// </summary>
    Task<Bill?> Handle(CreateBillCommand command);
}
