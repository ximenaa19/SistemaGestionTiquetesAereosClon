// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Cities\Infrastructure\Entity\CityEntityConfiguration.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GestionAerolineas.src.Modules.Regions.Infrastructure.Entity;

namespace GestionAerolineas.src.Modules.Cities.Infrastructure.Entity;

public sealed class CityEntityConfiguration : IEntityTypeConfiguration<CityEntity>
{
    public void Configure(EntityTypeBuilder<CityEntity> builder)
    {
        builder.ToTable("cities");

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
            .Property(x => x.RegionId)
            .HasColumnName("region_id")
            .HasColumnType("int")
            .IsRequired();
        builder
            .HasOne<RegionEntity>()
            .WithMany()
            .HasForeignKey(x => x.RegionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}


