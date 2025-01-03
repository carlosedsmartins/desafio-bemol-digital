using System.ComponentModel.DataAnnotations;

namespace PaymentIntentService.Application.DTOs;

public class CreatePaymentIntentDto
{
    [Required] [StringLength(100)] public required string PayerDocument { get; set; }

    [Required]
    [Range(0.01, double.MaxValue)]
    public required decimal Amount { get; set; }

    [StringLength(500)] public string? Description { get; set; }

    [Required] [StringLength(50)] public required string PaymentMethod { get; set; }
}