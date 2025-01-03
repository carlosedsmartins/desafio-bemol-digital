using System.ComponentModel.DataAnnotations;

namespace PaymentProcessorService.Application.DTOs;

public class ProcessedPaymentByIdDto
{
    [Required] public required Guid Id { get; set; }
}