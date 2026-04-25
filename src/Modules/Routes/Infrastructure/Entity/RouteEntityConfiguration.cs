// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Routes\Infrastructure\Entity\RouteEntityConfiguration.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Airports.Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionAerolineas.src.Modules.Routes.Infrastructure.Entity;

public sealed class RouteEntityConfiguration : IEntityTypeConfiguration<RouteEntity>
{
    public void Configure(EntityTypeBuilder<RouteEntity> builder)
    {
        builder.ToTable("routes");

        builder.HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .HasColumnName("id")
            .HasColumnType("int")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder
            .Property(x => x.OriginAirportId)
            .HasColumnName("aeropuerto_origen_id")
            .HasColumnType("int")
            .IsRequired();

        builder
            .Property(x => x.DestinationAirportId)
            .HasColumnName("aeropuerto_destino_id")
            .HasColumnType("int")
            .IsRequired();

        builder
            .Property(x => x.DistanceKm)
            .HasColumnName("distancia_km")
            .HasColumnType("int");

        builder
            .Property(x => x.EstimatedDurationMin)
            .HasColumnName("duracion_estimada_min")
            .HasColumnType("int");

        builder
            .HasIndex(x => new { x.OriginAirportId, x.DestinationAirportId })
            .IsUnique();

        builder
            .HasOne<AirportEntity>()
            .WithMany()
            .HasForeignKey(x => x.OriginAirportId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne<AirportEntity>()
            .WithMany()
            .HasForeignKey(x => x.DestinationAirportId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

