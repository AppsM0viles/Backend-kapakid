using FinTrackBack.Authentication.Domain.Entities;
using FinTrackBack.Documents.Domain.Entities;
using FinTrackBack.Documents.Infrastructure.Persistence.DbContext;
using FinTrackBack.Notifications.Domain.Entities;
using FinTrackBack.Notifications.Infrastructure.Persistence.DbContext;
using FinTrackBack.Payments.Domain.Entities;
using FinTrackBack.Support.Domain.Entities;
using FinTrackBack.Support.Infrastructure.Persistence.DbContext;
using FinTrackBack.Wallet.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinTrackBack.Authentication.Infrastructure.Persistence.DbContext;

public class FinTrackBackDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public FinTrackBackDbContext(DbContextOptions<FinTrackBackDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Document> Documents { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<SupportTicket> SupportTickets { get; set; }
    public DbSet<PaymentCard> PaymentCards { get; set; }
    public DbSet<TransportCard> TransportCards { get; set; }
    public DbSet<PaymentTransaction> PaymentTransactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>().ToTable("Users");
        modelBuilder.Entity<Payment>().ToTable("Payments");
        modelBuilder.Entity<PaymentCard>().ToTable("PaymentCards");
        modelBuilder.Entity<TransportCard>().ToTable("TransportCards");
        modelBuilder.Entity<PaymentTransaction>().ToTable("PaymentTransactions");

        modelBuilder.ApplyConfiguration(new DocumentConfiguration());
        modelBuilder.ApplyConfiguration(new NotificationConfiguration());
        modelBuilder.ApplyConfiguration(new SupportTicketConfiguration());
    }
}