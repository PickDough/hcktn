using Microsoft.EntityFrameworkCore;

namespace hcktn.infrastructure.db.context;

public class HcktnContext(DbContextOptions<HcktnContext> options) : DbContext(options)
{
    public DbSet<CityDb> Cities { get; set; }
    public DbSet<TagDb> Tags { get; set; }
    public DbSet<OrganisationDb> Organisations { get; set; }
    public DbSet<EventDb> Events { get; set; }
    public DbSet<EventTagDb> EventTags { get; set; }
    public DbSet<EventImageDb> EventImages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EventTagDb>()
            .HasKey(et => new { et.EventId, et.TagId });
    }
}
