using System.ComponentModel.DataAnnotations.Schema;
using prueba.bounded_context.Domain.Model;
using prueba.bounded_context.Domain.Model.Commands;
using EntityFrameworkCore.CreatedUpdatedDate.Interfaces;

namespace prueba.bounded_context.Domain.Model.Aggregates;

/// <summary>
///     Bill aggregate root
/// </summary>
public partial class Bill : IEntityWithCreatedUpdatedDate
{
    protected Bill() { }

    public Bill(CreateBillCommand command)
    {
        Customer = command.Customer;
        ServiceId = command.ServiceId;
        Plate = command.Plate;
        Emission = command.Emission;
        Invoice = new Invoice
        {
            SerialNumber = command.InvoiceSerialNumber,
            SequentialNumber = command.InvoiceSequentialNumber
        };
        Amount = command.Amount;
        Adviser = command.Adviser;
    }

    public int BillNumber { get; private set; }
    public string Customer { get; private set; } = string.Empty;
    public int ServiceId { get; private set; }
    public string Plate { get; private set; } = string.Empty;
    public DateTime Emission { get; private set; }
    public Invoice Invoice { get; private set; } = null!;
    public double Amount { get; private set; }
    public string Adviser { get; private set; } = string.Empty;
}

/// <summary>
///     Partial class for audit properties
/// </summary>
public partial class Bill
{
    [Column("created_at")]
    public DateTimeOffset? CreatedDate { get; set; }

    [Column("updated_at")]
    public DateTimeOffset? UpdatedDate { get; set; }
}
