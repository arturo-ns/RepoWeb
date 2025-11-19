using System.ComponentModel.DataAnnotations;

namespace prueba.bounded_context.Interfaces.REST.Resources;

/// <summary>
///     Resource for creating a bill
/// </summary>
public record CreateBillResource(
    [Required(ErrorMessage = "Customer is required")]
    [MaxLength(100, ErrorMessage = "Customer must not exceed 100 characters")]
    string Customer,
    
    [Required(ErrorMessage = "ServiceId is required")]
    int ServiceId,
    
    [MaxLength(10, ErrorMessage = "Plate must not exceed 10 characters")]
    string Plate,
    
    [Required(ErrorMessage = "Emission date is required")]
    DateTime Emission,
    
    [Required(ErrorMessage = "Invoice Serial Number is required")]
    [MaxLength(10, ErrorMessage = "Invoice Serial Number must not exceed 10 characters")]
    string InvoiceSerialNumber,
    
    [Required(ErrorMessage = "Invoice Sequential Number is required")]
    [MaxLength(10, ErrorMessage = "Invoice Sequential Number must not exceed 10 characters")]
    string InvoiceSequentialNumber,
    
    [Required(ErrorMessage = "Amount is required")]
    double Amount,
    
    [Required(ErrorMessage = "Adviser is required")]
    [MaxLength(100, ErrorMessage = "Adviser must not exceed 100 characters")]
    string Adviser
);
