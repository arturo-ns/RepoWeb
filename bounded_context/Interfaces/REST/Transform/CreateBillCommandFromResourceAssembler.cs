using prueba.bounded_context.Domain.Model.Commands;
using prueba.bounded_context.Interfaces.REST.Resources;

namespace prueba.bounded_context.Interfaces.REST.Transform;

/// <summary>
///     Assembler to transform CreateBillResource to CreateBillCommand
/// </summary>
public static class CreateBillCommandFromResourceAssembler
{
    public static CreateBillCommand ToCommandFromResource(CreateBillResource resource) =>
        new CreateBillCommand(
            resource.Customer,
            resource.ServiceId,
            resource.Plate,
            resource.Emission,
            resource.InvoiceSerialNumber,
            resource.InvoiceSequentialNumber,
            resource.Amount,
            resource.Adviser
        );
}
