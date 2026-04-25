// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\RouteStops\Infrastructure\Entity\RouteStopEntityConfiguration.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Airports.Infrastructure.Entity;
using GestionAerolineas.src.Modules.Routes.Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionAerolineas.src.Modules.RouteStops.Infrastructure.Entity;

public sealed class RouteStopEntityConfiguration : IEntityTypeConfiguration<RouteStopEntity>
{
    public void Configure(EntityTypeBuilder<RouteStopEntity> builder)
    {
        builder.ToTable("routestops");

        builder.HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .HasColumnName("id")
            .HasColumnType("int")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder
            .Property(x => x.RouteId)
            .HasColumnName("ruta_id")
            .HasColumnType("int")
            .IsRequired();

        builder
            .Property(x => x.StopAirportId)
            .HasColumnName("aeropuerto_escala_id")
            .HasColumnType("int")
            .IsRequired();

        builder
            .Property(x => x.Order)
            .HasColumnName("orden")
            .HasColumnType("int")
            .IsRequired();

        builder
            .Property(x => x.DurationMinutes)
            .HasColumnName("duracion_escala_min")
            .HasColumnType("int")
            .IsRequired();

        builder
            .HasIndex(x => new { x.RouteId, x.Order })
            .IsUnique();

        builder
            .HasOne<RouteEntity>()
            .WithMany()
            .HasForeignKey(x => x.RouteId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne<AirportEntity>()
            .WithMany()
            .HasForeignKey(x => x.StopAirportId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

