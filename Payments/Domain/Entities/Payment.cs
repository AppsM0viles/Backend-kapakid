namespace FinTrackBack.Payments.Domain.Entities
{
    /// <summary>
    /// Entidad que representa un pago realizado por un usuario.
    /// </summary>
    public class Payment
    {
        public Guid Id { get; set; }

        // 💳 Detalle del pago
        public string Servicio { get; set; } = string.Empty;
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }

        // 👤 Relación con el usuario (GUID)
        public Guid UserId { get; set; }
    }
}
