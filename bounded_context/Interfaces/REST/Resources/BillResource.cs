namespace prueba.bounded_context.Interfaces.REST.Resources;

/// <summary>
///     Resource for bill response (excludes plate and adviser)
/// </summary>
public record BillResource(
    int BillNumber,
    string Customer,
    int ServiceId,
    DateTime Emission,
    InvoiceResource Invoice,
    double Amount
);

/// <summary>
///     Resource for invoice owned attribute
/// </summary>
public record InvoiceResource(
    string SerialNumber,
    string SequentialNumber
);
