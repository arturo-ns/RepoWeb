using prueba.bounded_context.Domain.Model.Aggregates;
using prueba.bounded_context.Interfaces.REST.Resources;

namespace prueba.bounded_context.Interfaces.REST.Transform;

/// <summary>
///     Assembler to transform Bill entity to BillResource
/// </summary>
public static class BillResourceFromEntityAssembler
{
    public static BillResource ToResourceFromEntity(Bill entity) =>
        new BillResource(
            entity.BillNumber,
            entity.Customer,
            entity.ServiceId,
            entity.Emission,
            new InvoiceResource(
                entity.Invoice.SerialNumber,
                entity.Invoice.SequentialNumber
            ),
            entity.Amount
        );
}
