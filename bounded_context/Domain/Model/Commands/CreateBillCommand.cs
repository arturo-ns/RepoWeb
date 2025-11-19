namespace prueba.bounded_context.Domain.Model.Commands;

/// <summary>
///     Command to create a new bill
/// </summary>
public record CreateBillCommand(
    string Customer,
    int ServiceId,
    string Plate,
    DateTime Emission,
    string InvoiceSerialNumber,
    string InvoiceSequentialNumber,
    double Amount,
    string Adviser
);
