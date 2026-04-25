// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Airports\Infrastructure\Entity\AirportEntityConfiguration.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Cities.Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionAerolineas.src.Modules.Airports.Infrastructure.Entity;

public sealed class AirportEntityConfiguration : IEntityTypeConfiguration<AirportEntity>
{
    public void Configure(EntityTypeBuilder<AirportEntity> builder)
    {
        builder.ToTable("airports");

        builder.HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .HasColumnName("id")
            .HasColumnType("int")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder
            .Property(x => x.Name)
            .HasColumnName("nombre")
            .HasColumnType("varchar(150)")
            .IsRequired();

        builder
            .Property(x => x.IataCode)
            .HasColumnName("codigo_iata")
            .HasColumnType("varchar(3)")
            .IsRequired();

        builder
            .Property(x => x.IcaoCode)
            .HasColumnName("codigo_icao")
            .HasColumnType("varchar(4)");

        builder
            .Property(x => x.CityId)
            .HasColumnName("ciudad_id")
            .HasColumnType("int")
            .IsRequired();

        builder.HasIndex(x => x.IataCode).IsUnique();
        builder.HasIndex(x => x.IcaoCode).IsUnique();

        builder
            .HasOne<CityEntity>()
            .WithMany()
            .HasForeignKey(x => x.CityId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
