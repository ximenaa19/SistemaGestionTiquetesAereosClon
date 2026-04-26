// [DocHeader]
// Modulo: Reservations
// Capa: Infrastructure
// Archivo: src\Modules\Reservations\Infrastructure\Entity\ReservationWaitlistEntityConfiguration.cs
// Responsabilidad: Configuracion EF Core de tabla reservation_waitlist.
using GestionAerolineas.src.Modules.Flights.Infrastructure.Entity;
using GestionAerolineas.src.Modules.ReservationFlights.Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionAerolineas.src.Modules.Reservations.Infrastructure.Entity;

public sealed class ReservationWaitlistEntityConfiguration : IEntityTypeConfiguration<ReservationWaitlistEntity>
{
    public void Configure(EntityTypeBuilder<ReservationWaitlistEntity> builder)
    {
        builder.ToTable("reservation_waitlist");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int").ValueGeneratedOnAdd().IsRequired();
        builder.Property(x => x.ReservationId).HasColumnName("reservation_id").HasColumnType("int").IsRequired();
        builder.Property(x => x.ReservationFlightId).HasColumnName("reservation_flight_id").HasColumnType("int").IsRequired();
        builder.Property(x => x.RequestedFlightId).HasColumnName("requested_flight_id").HasColumnType("int").IsRequired();
        builder.Property(x => x.RequestedAt).HasColumnName("requested_at").HasColumnType("datetime").IsRequired();
        builder.Property(x => x.QueueOrder).HasColumnName("queue_order").HasColumnType("int").IsRequired();
        builder.Property(x => x.Status).HasColumnName("status").HasColumnType("varchar(30)").HasMaxLength(30).IsRequired();
        builder.Property(x => x.Reason).HasColumnName("reason").HasColumnType("varchar(255)").HasMaxLength(255);
        builder.Property(x => x.ProcessedAt).HasColumnName("processed_at").HasColumnType("datetime");

        builder.HasIndex(x => new { x.RequestedFlightId, x.Status, x.QueueOrder }).HasDatabaseName("ix_waitlist_requested_status_order");
        builder.HasIndex(x => new { x.ReservationFlightId, x.RequestedFlightId, x.Status }).HasDatabaseName("ix_waitlist_unique_pending");

        builder.HasOne<ReservationEntity>().WithMany().HasForeignKey(x => x.ReservationId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<ReservationFlightEntity>().WithMany().HasForeignKey(x => x.ReservationFlightId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<FlightEntity>().WithMany().HasForeignKey(x => x.RequestedFlightId).OnDelete(DeleteBehavior.Restrict);
    }
}

