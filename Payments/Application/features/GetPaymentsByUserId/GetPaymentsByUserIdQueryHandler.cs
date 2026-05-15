using FinTrackBack.Payments.Application.DTOs;
using FinTrackBack.Payments.Domain.Interfaces;

namespace FinTrackBack.Payments.Application.Features.GetPaymentsByUserId
{
    /// <summary>
    /// Maneja la obtención de pagos por ID de usuario.
    /// </summary>
    public class GetPaymentsByUserIdQueryHandler
    {
        private readonly IPaymentRepository _repository;

        public GetPaymentsByUserIdQueryHandler(IPaymentRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<PaymentDto>> HandleAsync(GetPaymentsByUserIdQuery query)
        {
            var payments = await _repository.GetPaymentsByUserIdAsync(query.UserId);

            return payments.Select(p => new PaymentDto
            {
                Id = p.Id,
                Servicio = p.Servicio,
                Monto = p.Monto,
                Fecha = p.Fecha,
                UserId = p.UserId
            });
        }
    }
}