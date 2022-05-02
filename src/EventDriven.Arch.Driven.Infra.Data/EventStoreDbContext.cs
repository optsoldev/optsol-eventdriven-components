using Microsoft.EntityFrameworkCore;
using Optsol.EventDriven.Components.Driven.Infra.Data;

namespace EventDriven.Arch.Driven.Infra.Data;

public class EventStoreDbContext : DbContext
{
    public EventStoreDbContext(DbContextOptions<EventStoreDbContext> options) : base(options) { }

    public DbSet<PersistentEvent<string>>?  Beneficiarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.Entity<PersistentEvent>().HasKey(k => new { k.ModelId, k.ModelVersion });
}