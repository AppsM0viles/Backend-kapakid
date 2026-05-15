using FinTrackBack.Authentication.Domain.Entities;
using FinTrackBack.Documents.Domain.Entities;
using FinTrackBack.Documents.Infrastructure.Persistence.DbContext;
using FinTrackBack.Notifications.Domain.Entities;
using FinTrackBack.Notifications.Infrastructure.Persistence.DbContext;
using FinTrackBack.Payments.Domain.Entities;
using FinTrackBack.Support.Domain.Entities;
using FinTrackBack.Support.Infrastructure.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;

namespace FinTrackBack.Authentication.Infrastructure.Persistence.DbContext;

public class FinTrackBackDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public FinTrackBackDbContext(DbContextOptions<FinTrackBackDbContext> options) : base(options)
    {
    }

    // 👤 Usuarios
    public DbSet<User> Users { get; set; }

    // 💳 Pagos
    public DbSet<Payment> Payments { get; set; }

    // 📄 Documentos
    public DbSet<Document> Documents { get; set; }

    // 🔔 Notificaciones
    public DbSet<Notification> Notifications { get; set; }

    // 🎫 Tickets de soporte
    public DbSet<SupportTicket> SupportTickets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>().ToTable("Users");
        modelBuilder.Entity<Payment>().ToTable("Payments");

        modelBuilder.ApplyConfiguration(new DocumentConfiguration());
        modelBuilder.ApplyConfiguration(new NotificationConfiguration());
        modelBuilder.ApplyConfiguration(new SupportTicketConfiguration());
    }
}
