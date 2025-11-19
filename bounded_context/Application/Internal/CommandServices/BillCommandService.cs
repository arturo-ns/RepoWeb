using prueba.bounded_context.Domain.Model;
using prueba.bounded_context.Domain.Model.Aggregates;
using prueba.bounded_context.Domain.Model.Commands;
using prueba.bounded_context.Domain.Repositories;
using prueba.bounded_context.Domain.Services;
using prueba.Shared.Domain.Repositories;

namespace prueba.bounded_context.Application.Internal.CommandServices;

/// <summary>
///     Command service implementation for Bill aggregate
/// </summary>
public class BillCommandService(
    IBillRepository repository,
    IUnitOfWork unitOfWork) : IBillCommandService
{
    public async Task<Bill?> Handle(CreateBillCommand command)
    {
        // Validación: No permite dos bills con el mismo invoice
        var existingBill = await repository.FindByInvoiceAsync(
            command.InvoiceSerialNumber, 
            command.InvoiceSequentialNumber);
        
        if (existingBill != null)
            throw new Exception("A bill with the same invoice already exists");

        // Validación: amount debe ser mayor que cero
        if (command.Amount <= 0)
            throw new Exception("Amount must be greater than zero");

        // Validación: La fecha de emisión no puede ser menor a la fecha del sistema
        if (command.Emission.Date < DateTime.Now.Date)
            throw new Exception("Emission date cannot be less than the current system date");

        // Validación: serviceId debe ser uno de los valores válidos del enum
        if (!Enum.IsDefined(typeof(EService), command.ServiceId))
            throw new Exception($"ServiceId must be one of: {EService.MaintenanceService}, {EService.BodyworkAndPaint}, {EService.Accessories}, {EService.SpareParts}");

        // Crear la entidad
        var bill = new Bill(command);

        // Persistir
        await repository.AddAsync(bill);
        await unitOfWork.CompleteAsync();

        return bill;
    }
}
