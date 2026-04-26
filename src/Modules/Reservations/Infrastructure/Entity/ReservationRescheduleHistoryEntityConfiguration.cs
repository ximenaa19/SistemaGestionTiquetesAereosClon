// [DocHeader]
// Modulo: Reservations
// Capa: Infrastructure
// Archivo: src\Modules\Reservations\Infrastructure\Entity\ReservationRescheduleHistoryEntityConfiguration.cs
// Responsabilidad: Configuracion EF Core de tabla reservation_reschedule_history.
using GestionAerolineas.src.Modules.Flights.Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionAerolineas.src.Modules.Reservations.Infrastructure.Entity;

public sealed class ReservationRescheduleHistoryEntityConfiguration : IEntityTypeConfiguration<ReservationRescheduleHistoryEntity>
{
    public void Configure(EntityTypeBuilder<ReservationRescheduleHistoryEntity> builder)
    {
        builder.ToTable("reservation_reschedule_history");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int").ValueGeneratedOnAdd().IsRequired();
        builder.Property(x => x.ReservationId).HasColumnName("reservation_id").HasColumnType("int").IsRequired();
        builder.Property(x => x.OldFlightId).HasColumnName("old_flight_id").HasColumnType("int").IsRequired();
        builder.Property(x => x.NewFlightId).HasColumnName("new_flight_id").HasColumnType("int").IsRequired();
        builder.Property(x => x.ChangedAt).HasColumnName("changed_at").HasColumnType("datetime").IsRequired();
        builder.Property(x => x.Reason).HasColumnName("reason").HasColumnType("varchar(255)").HasMaxLength(255);
        builder.Property(x => x.ActionStatus).HasColumnName("action_status").HasColumnType("varchar(50)").HasMaxLength(50).IsRequired();

        builder.HasIndex(x => new { x.ReservationId, x.ChangedAt }).HasDatabaseName("ix_reschedule_history_reservation_date");

        builder.HasOne<ReservationEntity>().WithMany().HasForeignKey(x => x.ReservationId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<FlightEntity>().WithMany().HasForeignKey(x => x.OldFlightId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<FlightEntity>().WithMany().HasForeignKey(x => x.NewFlightId).OnDelete(DeleteBehavior.Restrict);
    }
}

