using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace Event_managment.Models;

public partial class EventManagement2Context : DbContext
{
    public EventManagement2Context()
    {
    }

    public EventManagement2Context(DbContextOptions<EventManagement2Context> options)
        : base(options)
    {
    }

    public virtual DbSet<TbAccessList> TbAccessLists { get; set; }

    public virtual DbSet<TbEvent> TbEvents { get; set; }

    public virtual DbSet<TbEventParticipant> TbEventParticipants { get; set; }

    public virtual DbSet<TbLocation> TbLocations { get; set; }

    public virtual DbSet<TbParticipant> TbParticipants { get; set; }

    public virtual DbSet<VwEventParticipationSummary> VwEventParticipationSummaries { get; set; }

    public virtual DbSet<VwVenueCapacityStatus> VwVenueCapacityStatuses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<TbAccessList>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
            entity.ToTable("tb_access_list");
            entity.HasIndex(e => e.IpAddress, "ip_address_UNIQUE").IsUnique();
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt).HasMaxLength(6).HasColumnName("created_at");
            entity.Property(e => e.IpAddress).HasMaxLength(45).HasColumnName("ip_address");
            entity.Property(e => e.Reason).HasMaxLength(255).HasColumnName("reason");
            entity.Property(e => e.Type).HasMaxLength(10).HasColumnName("type");
        });

        modelBuilder.Entity<TbEvent>(entity =>
        {
            entity.HasKey(e => e.EventId).HasName("PRIMARY");
            entity.ToTable("tb_events");
            entity.HasIndex(e => e.LocationId, "FK_tb_events_tb_locations");
            entity.HasIndex(e => new { e.EventName, e.EventDate }, "UQ_tb_events_Name_Date").IsUnique();
            entity.Property(e => e.EventId).HasColumnName("event_id");
            entity.Property(e => e.CreatedAt).HasColumnType("timestamp").HasColumnName("created_at");
            entity.Property(e => e.Description).HasColumnType("text").HasColumnName("description");
            entity.Property(e => e.EventDate).HasColumnType("datetime").HasColumnName("event_date");
            entity.Property(e => e.EventName).HasMaxLength(200).HasColumnName("event_name");
            entity.Property(e => e.LocationId).HasColumnName("location_id");
            entity.Property(e => e.UpdatedAt).HasColumnType("timestamp").HasColumnName("updated_at");

            entity.HasOne(d => d.Location)
                .WithMany(p => p.TbEvents)
                .HasForeignKey(d => d.LocationId)
                .OnDelete(DeleteBehavior.Restrict) // Change this line
                .HasConstraintName("FK_tb_events_tb_locations");
        });

        modelBuilder.Entity<TbEventParticipant>(entity =>
        {
            entity.HasKey(e => e.EventParticipantId).HasName("PRIMARY");
            entity.ToTable("tb_event_participants");
            entity.HasIndex(e => e.EventId, "FK_EventParticipants_Event");
            entity.HasIndex(e => e.ParticipantId, "FK_EventParticipants_Participant");
            entity.Property(e => e.EventParticipantId).HasColumnName("event_participant_id");
            entity.Property(e => e.EventId).HasColumnName("event_id");
            entity.Property(e => e.ParticipantId).HasColumnName("participant_id");

            entity.HasOne(d => d.TbEvent)
                .WithMany(p => p.EventParticipants)
                .HasForeignKey(d => d.EventId)
                .OnDelete(DeleteBehavior.Restrict) // Change this line
                .HasConstraintName("FK_EventParticipants_Event");

            entity.HasOne(d => d.TbParticipant)
                .WithMany(p => p.EventParticipants)
                .HasForeignKey(d => d.ParticipantId)
                .OnDelete(DeleteBehavior.Restrict) // Change this line
                .HasConstraintName("FK_EventParticipants_Participant");
        });

        modelBuilder.Entity<TbLocation>(entity =>
        {
            entity.HasKey(e => e.LocationId).HasName("PRIMARY");
            entity.ToTable("tb_locations");
            entity.HasIndex(e => new { e.LocationName, e.Address }, "UQ_tb_locations_Name_Address")
                .IsUnique()
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 100 });

            entity.Property(e => e.LocationId).HasColumnName("location_id");
            entity.Property(e => e.Address).HasMaxLength(200).HasColumnName("address");
            entity.Property(e => e.Capacity).HasColumnName("capacity");
            entity.Property(e => e.City).HasMaxLength(100).HasColumnName("city");
            entity.Property(e => e.LocationName).HasMaxLength(100).HasColumnName("location_name");
            entity.Property(e => e.State).HasMaxLength(100).HasColumnName("state");
            entity.Property(e => e.ZipCode).HasMaxLength(20).HasColumnName("zip_code");
        });

        modelBuilder.Entity<TbParticipant>(entity =>
        {
            entity.HasKey(e => e.ParticipantId).HasName("PRIMARY");
            entity.ToTable("tb_participants");
            entity.HasIndex(e => e.Email, "UQ_tb_participants_Email").IsUnique();
            entity.HasIndex(e => e.Phone, "UQ_tb_participants_Phone").IsUnique();
            entity.Property(e => e.ParticipantId).HasColumnName("participant_id");
            entity.Property(e => e.CreatedAt).HasColumnType("timestamp").HasColumnName("created_at");
            entity.Property(e => e.Email).HasMaxLength(100).HasColumnName("email");
            entity.Property(e => e.ParticipantName).HasMaxLength(100).HasColumnName("participant_name");
            entity.Property(e => e.Phone).HasMaxLength(20).HasColumnName("phone");
            entity.Property(e => e.UpdatedAt).HasColumnType("timestamp").HasColumnName("updated_at");
        });

        modelBuilder.Entity<VwEventParticipationSummary>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_event_participation_summary");

            entity.Property(e => e.EventId).HasColumnName("event_id");
            entity.Property(e => e.EventName)
                .HasMaxLength(200)
                .HasColumnName("event_name");
            entity.Property(e => e.ParticipantCount).HasColumnName("participant_count");
        });

        modelBuilder.Entity<VwVenueCapacityStatus>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_venue_capacity_status");

            entity.Property(e => e.Capacity).HasColumnName("capacity");
            entity.Property(e => e.CapacityStatus)
                .HasMaxLength(9)
                .HasDefaultValueSql("''")
                .HasColumnName("capacity_status");
            entity.Property(e => e.CurrentParticipants).HasColumnName("current_participants");
            entity.Property(e => e.LocationId).HasColumnName("location_id");
            entity.Property(e => e.LocationName)
                .HasMaxLength(100)
                .HasColumnName("location_name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
