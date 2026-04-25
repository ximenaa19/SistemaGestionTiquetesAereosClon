// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\AircraftManufacturers\Infrastructure\Entity\AircraftManufacturerEntityConfiguration.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Countries.Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionAerolineas.src.Modules.AircraftManufacturers.Infrastructure.Entity;

public sealed class AircraftManufacturerEntityConfiguration : IEntityTypeConfiguration<AircraftManufacturerEntity>
{
    public void Configure(EntityTypeBuilder<AircraftManufacturerEntity> builder)
    {
        builder.ToTable("aircraftmanufacturers");

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
            .HasColumnType("varchar(100)")
            .IsRequired();

        builder
            .Property(x => x.CountryId)
            .HasColumnName("country_id")
            .HasColumnType("int")
            .IsRequired();

        builder.HasIndex(x => x.Name).IsUnique();

        builder
            .HasOne<CountryEntity>()
            .WithMany()
            .HasForeignKey(x => x.CountryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

