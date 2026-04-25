// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\FlightSeats\Infrastructure\Entity\FlightSeatEntityConfiguration.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.CabinTypes.Infrastructure.Entity;
using GestionAerolineas.src.Modules.Flights.Infrastructure.Entity;
using GestionAerolineas.src.Modules.SeatLocationTypes.Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionAerolineas.src.Modules.FlightSeats.Infrastructure.Entity;

public sealed class FlightSeatEntityConfiguration : IEntityTypeConfiguration<FlightSeatEntity>
{
    public void Configure(EntityTypeBuilder<FlightSeatEntity> builder)
    {
        builder.ToTable("flightseats");

        builder.HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .HasColumnName("id")
            .HasColumnType("int")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder
            .Property(x => x.FlightId)
            .HasColumnName("vuelo_id")
            .HasColumnType("int")
            .IsRequired();

        builder
            .Property(x => x.SeatCode)
            .HasColumnName("codigo_asiento")
            .HasColumnType("varchar(5)")
            .IsRequired();

        builder
            .Property(x => x.CabinTypeId)
            .HasColumnName("tipo_cabina_id")
            .HasColumnType("int")
            .IsRequired();

        builder
            .Property(x => x.LocationTypeId)
            .HasColumnName("tipo_ubicacion_id")
            .HasColumnType("int")
            .IsRequired();

        builder
            .Property(x => x.IsOccupied)
            .HasColumnName("esta_ocupado")
            .HasColumnType("tinyint(1)")
            .IsRequired();

        builder
            .HasIndex(x => new { x.FlightId, x.SeatCode })
            .IsUnique();

        builder
            .HasOne<FlightEntity>()
            .WithMany()
            .HasForeignKey(x => x.FlightId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne<CabinTypeEntity>()
            .WithMany()
            .HasForeignKey(x => x.CabinTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne<SeatLocationTypeEntity>()
            .WithMany()
            .HasForeignKey(x => x.LocationTypeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

