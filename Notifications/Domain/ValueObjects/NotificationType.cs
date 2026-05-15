namespace FinTrackBack.Notifications.Domain.ValueObjects;

public enum NotificationType
{
    System = 1,        // Mensajes del sistema
    Transaction = 2,   // Movimientos de dinero
    Goal = 3,          // Metas de ahorro / presupuesto
    Reminder = 4,      // Recordatorios (pagos, vencimientos)
    Security = 5       // Alertas de seguridad / login
}