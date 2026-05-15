using FinTrackBack.Payments.Domain.Entities;
using FinTrackBack.Payments.Domain.Interfaces;

namespace FinTrackBack.Payments.Application.Features.CreatePayment
{
    /// <summary>
    /// Maneja la creación de pagos.
    /// </summary>
    public class CreatePaymentCommandHandler
    {
        private readonly IPaymentRepository _repository;

        public CreatePaymentCommandHandler(IPaymentRepository repository)
        {
            _repository = repository;
        }

        public async Task<Guid> HandleAsync(CreatePaymentCommand command)
        {
            var dto = command.Payment;

            var payment = new Payment
            {
                Id = Guid.NewGuid(),
                Servicio = dto.Servicio,
                Monto = dto.Monto,
                Fecha = dto.Fecha,
                UserId = dto.UserId
            };

            var created = await _repository.AddPaymentAsync(payment);
            return created.Id;
        }
    }
}