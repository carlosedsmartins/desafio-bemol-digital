using System.ComponentModel.DataAnnotations;

namespace PaymentIntentService.Application.DTOs
{
    public class PaymentIntentCreateDTO
    {
        [Required]
        public decimal Amount { get; set; }
    }
}
