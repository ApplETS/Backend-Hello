using HelloAPI.Data.Entities;

using Microsoft.EntityFrameworkCore;
namespace HelloAPI.Data;

public class HelloContext: DbContext
{

    public HelloContext(DbContextOptions<HelloContext> options) : base(options)  { }

    public DbSet<Publication> Publications { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<Moderator> Moderators { get; set; }
    public DbSet<Organizer> Organizers { get; set; }
    public DbSet<Report> Reports { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<UserProfile> UserProfiles { get; set; } 

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Publication>().UseTptMappingStrategy();
        modelBuilder.Entity<Publication>()
            .HasMany(p => p.Tags)
            .WithMany(t => t.Publications);
        modelBuilder.Entity<Tag>()
            .HasMany(c => c.ChildrenTags)
            .WithMany(p => p.ParentTags)
            .UsingEntity(x => x.ToTable("TagsHierarchy"));

        modelBuilder.Entity<UserProfile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("userprofile_pkey");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("timezone('utc'::text, now())");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("timezone('utc'::text, now())");
        });
    }
}
