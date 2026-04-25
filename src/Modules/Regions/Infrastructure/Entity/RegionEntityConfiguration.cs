// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Regions\Infrastructure\Entity\RegionEntityConfiguration.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Countries.Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionAerolineas.src.Modules.Regions.Infrastructure.Entity;

public sealed class RegionEntityConfiguration : IEntityTypeConfiguration<RegionEntity>
{
    public void Configure(EntityTypeBuilder<RegionEntity> builder)
    {
        builder.ToTable("regions");

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
            .Property(x => x.Type)
            .HasColumnName("tipo")
            .HasColumnType("varchar(30)")
            .IsRequired();

        builder
            .Property(x => x.CountryId)
            .HasColumnName("pais_id")
            .HasColumnType("int")
            .IsRequired();
        builder
            .HasOne<CountryEntity>()
            .WithMany()
            .HasForeignKey(x => x.CountryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}


