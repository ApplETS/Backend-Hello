using System;
using System.Collections.Generic;

using api.core.data.entities;

using Microsoft.EntityFrameworkCore;

namespace api.core.data;

public partial class EventManagementContext : DbContext
{
    public EventManagementContext()
    {
    }

    public EventManagementContext(DbContextOptions<EventManagementContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<Moderator> Moderators { get; set; }

    public virtual DbSet<Organizer> Organizers { get; set; }

    public virtual DbSet<Publication> Publications { get; set; }

    public virtual DbSet<Report> Reports { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Event>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Moderator>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(now() AT TIME ZONE 'utc'::text)");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(now() AT TIME ZONE 'utc'::text)");
        });

        modelBuilder.Entity<Organizer>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(now() AT TIME ZONE 'utc'::text)");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(now() AT TIME ZONE 'utc'::text)");
        });

        modelBuilder.Entity<Publication>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(now() AT TIME ZONE 'utc'::text)");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(now() AT TIME ZONE 'utc'::text)");

            entity.HasOne(d => d.Moderator).WithMany(p => p.Publications).HasConstraintName("Publication_ModeratorId_fkey");

            entity.HasOne(d => d.Organizer).WithMany(p => p.Publications).HasConstraintName("Publication_OrganizerId_fkey");

            entity.HasMany(d => d.Tags).WithMany(p => p.Publications)
                .UsingEntity<Dictionary<string, object>>(
                    "PublicationTag",
                    r => r.HasOne<Tag>().WithMany().HasForeignKey("TagsId"),
                    l => l.HasOne<Publication>().WithMany().HasForeignKey("PublicationsId"),
                    j =>
                    {
                        j.HasKey("PublicationsId", "TagsId");
                        j.ToTable("PublicationTag");
                        j.HasIndex(new[] { "TagsId" }, "IX_PublicationTag_TagsId");
                    });
        });

        modelBuilder.Entity<Report>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(now() AT TIME ZONE 'utc'::text)");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(now() AT TIME ZONE 'utc'::text)");

            entity.HasOne(d => d.Publication).WithMany(p => p.Reports).HasConstraintName("Report_PublicationId_fkey");
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(now() AT TIME ZONE 'utc'::text)");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(now() AT TIME ZONE 'utc'::text)");

            entity.HasMany(d => d.ChildrenTags).WithMany(p => p.ParentTags)
                .UsingEntity<Dictionary<string, object>>(
                    "TagsHierarchy",
                    r => r.HasOne<Tag>().WithMany().HasForeignKey("ChildrenTagsId"),
                    l => l.HasOne<Tag>().WithMany().HasForeignKey("ParentTagsId"),
                    j =>
                    {
                        j.HasKey("ChildrenTagsId", "ParentTagsId");
                        j.ToTable("TagsHierarchy");
                        j.HasIndex(new[] { "ParentTagsId" }, "IX_TagsHierarchy_ParentTagsId");
                    });

            entity.HasMany(d => d.ParentTags).WithMany(p => p.ChildrenTags)
                .UsingEntity<Dictionary<string, object>>(
                    "TagsHierarchy",
                    r => r.HasOne<Tag>().WithMany().HasForeignKey("ParentTagsId"),
                    l => l.HasOne<Tag>().WithMany().HasForeignKey("ChildrenTagsId"),
                    j =>
                    {
                        j.HasKey("ChildrenTagsId", "ParentTagsId");
                        j.ToTable("TagsHierarchy");
                        j.HasIndex(new[] { "ParentTagsId" }, "IX_TagsHierarchy_ParentTagsId");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
