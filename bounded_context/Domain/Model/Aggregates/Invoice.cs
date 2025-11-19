using Microsoft.EntityFrameworkCore;

namespace prueba.bounded_context.Domain.Model.Aggregates;

/// <summary>
///     Owned attribute for Invoice (Serial Number and Sequential Number)
/// </summary>
[Owned]
public class Invoice
{
    public string SerialNumber { get; set; } = string.Empty;
    public string SequentialNumber { get; set; } = string.Empty;
}
