using Microsoft.EntityFrameworkCore;

namespace EventDriven.Arch.Driven.Infra.Data;

public class EventStoreDbContext : DbContext
{
    public EventStoreDbContext(DbContextOptions<EventStoreDbContext> options) : base(options) { }

    public DbSet<PersistentEvent>? Beneficiarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.Entity<PersistentEvent>().HasKey(k => new { k.ModelId, k.ModelVersion });
}