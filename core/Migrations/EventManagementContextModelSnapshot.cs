﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using api.core.data;

#nullable disable

namespace api.core.Migrations
{
    [DbContext(typeof(EventManagementContext))]
    partial class EventManagementContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("PublicationTag", b =>
                {
                    b.Property<Guid>("PublicationsId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("TagsId")
                        .HasColumnType("uuid");

                    b.HasKey("PublicationsId", "TagsId");

                    b.HasIndex(new[] { "TagsId" }, "IX_PublicationTag_TagsId");

                    b.ToTable("PublicationTag", (string)null);
                });

            modelBuilder.Entity("TagTag", b =>
                {
                    b.Property<Guid>("ChildrenTagsId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ParentTagsId")
                        .HasColumnType("uuid");

                    b.HasKey("ChildrenTagsId", "ParentTagsId");

                    b.ToTable("TagTag");
                });

            modelBuilder.Entity("TagsHierarchy", b =>
                {
                    b.Property<Guid>("ChildrenTagsId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ParentTagsId")
                        .HasColumnType("uuid");

                    b.HasKey("ChildrenTagsId", "ParentTagsId");

                    b.HasIndex(new[] { "ParentTagsId" }, "IX_TagsHierarchy_ParentTagsId");

                    b.ToTable("TagsHierarchy", (string)null);
                });

            modelBuilder.Entity("api.core.data.entities.Event", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("EventEndDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("EventStartDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("ReportCount")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Event");
                });

            modelBuilder.Entity("api.core.data.entities.Moderator", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("(now() AT TIME ZONE 'utc'::text)");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("(now() AT TIME ZONE 'utc'::text)");

                    b.HasKey("Id");

                    b.ToTable("Moderator");
                });

            modelBuilder.Entity("api.core.data.entities.Organizer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<string>("ActivityArea")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("(now() AT TIME ZONE 'utc'::text)");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("DiscordLink")
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FacebookLink")
                        .HasColumnType("text");

                    b.Property<string>("InstagramLink")
                        .HasColumnType("text");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("LinkedInLink")
                        .HasColumnType("text");

                    b.Property<string>("Organization")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ProfileDescription")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("RedditLink")
                        .HasColumnType("text");

                    b.Property<string>("TikTokLink")
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("(now() AT TIME ZONE 'utc'::text)");

                    b.Property<string>("WebSiteLink")
                        .HasColumnType("text");

                    b.Property<string>("XLink")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Organizer");
                });

            modelBuilder.Entity("api.core.data.entities.Publication", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<string>("Content")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("(now() AT TIME ZONE 'utc'::text)");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("ImageAltText")
                        .HasColumnType("text");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("text");

                    b.Property<Guid?>("ModeratorId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("OrganizerId")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("PublicationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Reason")
                        .HasColumnType("text");

                    b.Property<int>("State")
                        .HasColumnType("integer");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("(now() AT TIME ZONE 'utc'::text)");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "ModeratorId" }, "IX_Publication_ModeratorId");

                    b.HasIndex(new[] { "OrganizerId" }, "IX_Publication_OrganizerId");

                    b.ToTable("Publication");
                });

            modelBuilder.Entity("api.core.data.entities.Report", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<int>("Category")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("(now() AT TIME ZONE 'utc'::text)");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("PublicationId")
                        .HasColumnType("uuid");

                    b.Property<string>("Reason")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("(now() AT TIME ZONE 'utc'::text)");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "PublicationId" }, "IX_Report_PublicationId");

                    b.ToTable("Report");
                });

            modelBuilder.Entity("api.core.data.entities.Tag", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("(now() AT TIME ZONE 'utc'::text)");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("PriorityValue")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("(now() AT TIME ZONE 'utc'::text)");

                    b.HasKey("Id");

                    b.ToTable("Tag");
                });

            modelBuilder.Entity("PublicationTag", b =>
                {
                    b.HasOne("api.core.data.entities.Publication", null)
                        .WithMany()
                        .HasForeignKey("PublicationsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("api.core.data.entities.Tag", null)
                        .WithMany()
                        .HasForeignKey("TagsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TagsHierarchy", b =>
                {
                    b.HasOne("api.core.data.entities.Tag", null)
                        .WithMany()
                        .HasForeignKey("ChildrenTagsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("api.core.data.entities.Tag", null)
                        .WithMany()
                        .HasForeignKey("ParentTagsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("api.core.data.entities.Event", b =>
                {
                    b.HasOne("api.core.data.entities.Publication", "Publication")
                        .WithOne("Event")
                        .HasForeignKey("api.core.data.entities.Event", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Publication");
                });

            modelBuilder.Entity("api.core.data.entities.Publication", b =>
                {
                    b.HasOne("api.core.data.entities.Moderator", "Moderator")
                        .WithMany("Publications")
                        .HasForeignKey("ModeratorId")
                        .HasConstraintName("Publication_ModeratorId_fkey");

                    b.HasOne("api.core.data.entities.Organizer", "Organizer")
                        .WithMany("Publications")
                        .HasForeignKey("OrganizerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("Publication_OrganizerId_fkey");

                    b.Navigation("Moderator");

                    b.Navigation("Organizer");
                });

            modelBuilder.Entity("api.core.data.entities.Report", b =>
                {
                    b.HasOne("api.core.data.entities.Publication", "Publication")
                        .WithMany("Reports")
                        .HasForeignKey("PublicationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("Report_PublicationId_fkey");

                    b.Navigation("Publication");
                });

            modelBuilder.Entity("api.core.data.entities.Moderator", b =>
                {
                    b.Navigation("Publications");
                });

            modelBuilder.Entity("api.core.data.entities.Organizer", b =>
                {
                    b.Navigation("Publications");
                });

            modelBuilder.Entity("api.core.data.entities.Publication", b =>
                {
                    b.Navigation("Event");

                    b.Navigation("Reports");
                });
#pragma warning restore 612, 618
        }
    }
}
