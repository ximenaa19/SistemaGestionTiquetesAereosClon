// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\ReservationFlights\Infrastructure\Entity\ReservationFlightEntityConfiguration.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Flights.Infrastructure.Entity;
using GestionAerolineas.src.Modules.Reservations.Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionAerolineas.src.Modules.ReservationFlights.Infrastructure.Entity;

public sealed class ReservationFlightEntityConfiguration : IEntityTypeConfiguration<ReservationFlightEntity>
{
    public void Configure(EntityTypeBuilder<ReservationFlightEntity> builder)
    {
        builder.ToTable("reservationflights");

        builder.HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .HasColumnName("id")
            .HasColumnType("int")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder
            .Property(x => x.ReservationId)
            .HasColumnName("reserva_id")
            .HasColumnType("int")
            .IsRequired();

        builder
            .Property(x => x.FlightId)
            .HasColumnName("vuelo_id")
            .HasColumnType("int")
            .IsRequired();

        builder
            .Property(x => x.PartialAmount)
            .HasColumnName("valor_parcial")
            .HasColumnType("decimal(18,2)")
            .HasDefaultValue(0)
            .IsRequired();

        builder
            .HasIndex(x => new { x.ReservationId, x.FlightId })
            .IsUnique();

        builder
            .HasOne<ReservationEntity>()
            .WithMany()
            .HasForeignKey(x => x.ReservationId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne<FlightEntity>()
            .WithMany()
            .HasForeignKey(x => x.FlightId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

