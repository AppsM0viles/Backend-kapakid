namespace FinTrackBack.Payments.Application.DTOs
{
    /// <summary>
    /// Objeto de transferencia de datos (DTO) para pagos.
    /// </summary>
    public class PaymentDto
    {
        public Guid Id { get; set; }
        public string Servicio { get; set; } = string.Empty;
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }
        public Guid UserId { get; set; } // 👈 Importante: debe ser Guid
    }
}