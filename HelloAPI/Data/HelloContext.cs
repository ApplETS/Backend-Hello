using System.Reflection.Metadata;

using HelloAPI.Data.DataModels;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
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

    protected override void OnConfiguring(DbContextOptionsBuilder opt)
        => opt.UseNpgsql(
            "UserId=postgres;Password=USTTdY1rIJhOjHwA;Server=db.ekjsmadkoplhbuafidaa.supabase.co;Port=5432;Database=postgres");
    
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
        
    }
}
