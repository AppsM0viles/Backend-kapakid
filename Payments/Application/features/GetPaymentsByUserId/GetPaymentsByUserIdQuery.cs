namespace FinTrackBack.Payments.Application.Features.GetPaymentsByUserId
{
    /// <summary>
    /// Consulta para obtener pagos de un usuario específico.
    /// </summary>
    public class GetPaymentsByUserIdQuery
    {
        public Guid UserId { get; set; }
    }
}