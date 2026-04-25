// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\ReservationPassengers\Infrastructure\Entity\ReservationPassengerEntityConfiguration.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Passengers.Infrastructure.Entity;
using GestionAerolineas.src.Modules.ReservationFlights.Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionAerolineas.src.Modules.ReservationPassengers.Infrastructure.Entity;

public sealed class ReservationPassengerEntityConfiguration : IEntityTypeConfiguration<ReservationPassengerEntity>
{
    public void Configure(EntityTypeBuilder<ReservationPassengerEntity> builder)
    {
        builder.ToTable("reservationpassengers");

        builder.HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .HasColumnName("id")
            .HasColumnType("int")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder
            .Property(x => x.ReservationFlightId)
            .HasColumnName("reserva_vuelo_id")
            .HasColumnType("int")
            .IsRequired();

        builder
            .Property(x => x.PassengerId)
            .HasColumnName("pasajero_id")
            .HasColumnType("int")
            .IsRequired();

        builder
            .HasIndex(x => new { x.ReservationFlightId, x.PassengerId })
            .IsUnique();

        builder
            .HasOne<ReservationFlightEntity>()
            .WithMany()
            .HasForeignKey(x => x.ReservationFlightId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne<PassengerEntity>()
            .WithMany()
            .HasForeignKey(x => x.PassengerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

