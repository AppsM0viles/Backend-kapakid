using FinTrackBack.Payments.Application.DTOs;

namespace FinTrackBack.Payments.Application.Features.CreatePayment
{
    /// <summary>
    /// Comando para crear un nuevo pago.
    /// </summary>
    public class CreatePaymentCommand
    {
        public PaymentDto Payment { get; set; } = new();
    }
}