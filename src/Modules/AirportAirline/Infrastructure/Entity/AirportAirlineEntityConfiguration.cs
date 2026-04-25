// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\AirportAirline\Infrastructure\Entity\AirportAirlineEntityConfiguration.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Airlines.Infrastructure.Entity;
using GestionAerolineas.src.Modules.Airports.Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionAerolineas.src.Modules.AirportAirline.Infrastructure.Entity;

public sealed class AirportAirlineEntityConfiguration : IEntityTypeConfiguration<AirportAirlineEntity>
{
    public void Configure(EntityTypeBuilder<AirportAirlineEntity> builder)
    {
        builder.ToTable("airportairline");

        builder.HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .HasColumnName("id")
            .HasColumnType("int")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder
            .Property(x => x.AirportId)
            .HasColumnName("aeropuerto_id")
            .HasColumnType("int")
            .IsRequired();

        builder
            .Property(x => x.AirlineId)
            .HasColumnName("aerolinea_id")
            .HasColumnType("int")
            .IsRequired();

        builder
            .Property(x => x.Terminal)
            .HasColumnName("terminal")
            .HasColumnType("varchar(20)");

        builder
            .Property(x => x.StartDate)
            .HasColumnName("fecha_inicio")
            .HasColumnType("date")
            .IsRequired();

        builder
            .Property(x => x.EndDate)
            .HasColumnName("fecha_fin")
            .HasColumnType("date");

        builder
            .Property(x => x.IsActive)
            .HasColumnName("activa")
            .HasColumnType("tinyint(1)")
            .IsRequired();

        builder
            .HasIndex(x => new { x.AirportId, x.AirlineId })
            .IsUnique();

        builder
            .HasOne<AirportEntity>()
            .WithMany()
            .HasForeignKey(x => x.AirportId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne<AirlineEntity>()
            .WithMany()
            .HasForeignKey(x => x.AirlineId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

