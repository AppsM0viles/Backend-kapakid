using FinTrackBack.Payments.Domain.Entities;

namespace FinTrackBack.Payments.Domain.Interfaces
{
    /// <summary>
    /// Define las operaciones de acceso a datos para la entidad Payment.
    /// </summary>
    public interface IPaymentRepository
    {
        /// <summary>
        /// Obtiene todos los pagos asociados a un usuario específico.
        /// </summary>
        Task<IEnumerable<Payment>> GetPaymentsByUserIdAsync(Guid userId);

        /// <summary>
        /// Crea un nuevo pago.
        /// </summary>
        Task<Payment> AddPaymentAsync(Payment payment);
    }
}