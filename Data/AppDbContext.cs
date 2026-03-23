using Microsoft.EntityFrameworkCore;
using SalonBooking.Api.Entities;

namespace SalonBooking.Api.Data;

public class AppDbContext : DbContext
{
    public DbSet<ServiceEntity> Services => Set<ServiceEntity>();
    public DbSet<ClientEntity> Clients => Set<ClientEntity>();
    public DbSet<AppointmentEntity> Appointments => Set<AppointmentEntity>();
    public DbSet<WorkingTimeBlockEntity> WorkingTimeBlocks => Set<WorkingTimeBlockEntity>();
    public DbSet<ScheduleExceptionEntity> ScheduleExceptions => Set<ScheduleExceptionEntity>();
    public DbSet<AdminUserEntity> AdminUsers => Set<AdminUserEntity>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("public");

        modelBuilder.Entity<ServiceEntity>(entity =>
        {
            entity.ToTable("services");

            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();

            entity.Property(x => x.Name)
                .HasColumnName("name")
                .HasMaxLength(120)
                .IsRequired();

            entity.Property(x => x.DurationMinutes)
                .HasColumnName("duration_minutes");

            entity.Property(x => x.Price)
                .HasColumnName("price")
                .HasColumnType("numeric(10,2)");

            entity.Property(x => x.IsActive)
                .HasColumnName("is_active");
        });

        modelBuilder.Entity<ClientEntity>(entity =>
        {
            entity.ToTable("clients");

            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();

            entity.Property(x => x.FullName)
                .HasColumnName("full_name")
                .HasMaxLength(150)
                .IsRequired();

            entity.Property(x => x.Phone)
                .HasColumnName("phone")
                .HasMaxLength(30)
                .IsRequired();
        });

        modelBuilder.Entity<AppointmentEntity>(entity =>
        {
            entity.ToTable("appointments");

            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();

            entity.Property(x => x.ServiceId).HasColumnName("service_id");
            entity.Property(x => x.ClientId).HasColumnName("client_id");

            entity.Property(x => x.StartAt)
                .HasColumnName("start_at")
                .HasColumnType("timestamp with time zone");

            entity.Property(x => x.EndAt)
                .HasColumnName("end_at")
                .HasColumnType("timestamp with time zone");

            entity.Property(x => x.Status)
                .HasColumnName("status");
                
            entity.Property(x => x.BlockNumber)
                .HasColumnName("block_number");

            entity.Property(x => x.Notes)
                .HasColumnName("notes")
                .HasColumnType("text");

            entity.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("timestamp with time zone");

            entity.Property(x => x.DecisionAt)
                .HasColumnName("decision_at")
                .HasColumnType("timestamp with time zone");

            entity.Property(x => x.CancelReason)
                .HasColumnName("cancel_reason")
                .HasMaxLength(250);

            entity.HasOne(x => x.Client)
                .WithMany(x => x.Appointments)
                .HasForeignKey(x => x.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Service)
                .WithMany()
                .HasForeignKey(x => x.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(x => x.StartAt);
            entity.HasIndex(x => x.Status);
            entity.HasIndex(x => new { x.StartAt, x.Status });
        });

        modelBuilder.Entity<WorkingTimeBlockEntity>(entity =>
        {
            entity.ToTable("working_time_blocks");

            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();

            entity.Property(x => x.DayOfWeek).HasColumnName("day_of_week");
            entity.Property(x => x.BlockNumber).HasColumnName("block_number");
            entity.Property(x => x.StartTime).HasColumnName("start_time");
            entity.Property(x => x.EndTime).HasColumnName("end_time");
            entity.Property(x => x.IsEnabled).HasColumnName("is_enabled");

            entity.HasIndex(x => new { x.DayOfWeek, x.BlockNumber }).IsUnique();
        });

        modelBuilder.Entity<ScheduleExceptionEntity>(entity =>
        {
            entity.ToTable("schedule_exceptions");

            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();

            entity.Property(x => x.Date).HasColumnName("date");
            entity.Property(x => x.Type).HasColumnName("type");
            entity.Property(x => x.StartTime).HasColumnName("start_time");
            entity.Property(x => x.EndTime).HasColumnName("end_time");
            entity.Property(x => x.AllDay).HasColumnName("all_day");
            entity.Property(x => x.Reason)
                .HasColumnName("reason")
                .HasMaxLength(250);

            entity.HasIndex(x => x.Date);
        });

        modelBuilder.Entity<AdminUserEntity>(entity =>
        {
            entity.ToTable("admin_users");

            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();

            entity.Property(x => x.Username)
                .HasColumnName("username")
                .HasMaxLength(80)
                .IsRequired();

            entity.Property(x => x.PasswordHash)
                .HasColumnName("password_hash")
                .IsRequired();

            entity.HasIndex(x => x.Username).IsUnique();
        });
    }
}