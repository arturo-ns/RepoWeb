namespace prueba.bounded_context.Domain.Model.Queries;

/// <summary>
///     Query to get a bill by id
/// </summary>
public record GetBillByIdQuery(int BillNumber);
