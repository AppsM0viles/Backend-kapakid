using Microsoft.EntityFrameworkCore;
using FinTrackBack.Payments.Domain.Entities;
using FinTrackBack.Payments.Domain.Interfaces;
using FinTrackBack.Authentication.Infrastructure.Persistence.DbContext;

namespace FinTrackBack.Payments.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// Implementación del repositorio de pagos.
    /// </summary>
    public class PaymentRepository : IPaymentRepository
    {
        private readonly FinTrackBackDbContext _context;

        public PaymentRepository(FinTrackBackDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByUserIdAsync(Guid userId)
        {
            return await _context.Payments
                .Where(p => p.UserId == userId)
                .ToListAsync();
        }

        public async Task<Payment> AddPaymentAsync(Payment payment)
        {
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();
            return payment;
        }
    }
}