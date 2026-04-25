// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Flights\Infrastructure\Entity\FlightEntityConfiguration.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Aircraft.Infrastructure.Entity;
using GestionAerolineas.src.Modules.Airlines.Infrastructure.Entity;
using GestionAerolineas.src.Modules.FlightStates.Infrastructure.Entity;
using GestionAerolineas.src.Modules.Routes.Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionAerolineas.src.Modules.Flights.Infrastructure.Entity;

public sealed class FlightEntityConfiguration : IEntityTypeConfiguration<FlightEntity>
{
    public void Configure(EntityTypeBuilder<FlightEntity> builder)
    {
        builder.ToTable("flights");

        builder.HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .HasColumnName("id")
            .HasColumnType("int")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder
            .Property(x => x.Code)
            .HasColumnName("codigo_vuelo")
            .HasColumnType("varchar(10)")
            .IsRequired();

        builder.HasIndex(x => x.Code).IsUnique();

        builder
            .Property(x => x.AirlineId)
            .HasColumnName("aerolinea_id")
            .HasColumnType("int")
            .IsRequired();

        builder
            .Property(x => x.RouteId)
            .HasColumnName("ruta_id")
            .HasColumnType("int")
            .IsRequired();

        builder
            .Property(x => x.AircraftId)
            .HasColumnName("aeronave_id")
            .HasColumnType("int")
            .IsRequired();

        builder
            .Property(x => x.DepartureDateTime)
            .HasColumnName("fecha_salida")
            .HasColumnType("datetime")
            .IsRequired();

        builder
            .Property(x => x.EstimatedArrivalDateTime)
            .HasColumnName("fecha_llegada_estimada")
            .HasColumnType("datetime")
            .IsRequired();

        builder
            .Property(x => x.TotalCapacity)
            .HasColumnName("capacidad_total")
            .HasColumnType("int")
            .IsRequired();

        builder
            .Property(x => x.AvailableSeats)
            .HasColumnName("asientos_disponibles")
            .HasColumnType("int")
            .IsRequired();

        builder
            .Property(x => x.StateId)
            .HasColumnName("estado_vuelo_id")
            .HasColumnType("int")
            .IsRequired();

        builder
            .Property(x => x.RescheduledAt)
            .HasColumnName("reprogramado_en")
            .HasColumnType("datetime");

        builder
            .Property(x => x.CreatedAt)
            .HasColumnName("creado_en")
            .HasColumnType("datetime")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd();

        builder
            .Property(x => x.UpdatedAt)
            .HasColumnName("actualizado_en")
            .HasColumnType("datetime")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAddOrUpdate();

        builder
            .HasOne<AirlineEntity>()
            .WithMany()
            .HasForeignKey(x => x.AirlineId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne<RouteEntity>()
            .WithMany()
            .HasForeignKey(x => x.RouteId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne<AircraftEntity>()
            .WithMany()
            .HasForeignKey(x => x.AircraftId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne<FlightStateEntity>()
            .WithMany()
            .HasForeignKey(x => x.StateId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

