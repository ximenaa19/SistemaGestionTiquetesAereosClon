// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Airlines\Infrastructure\Entity\AirlineEntityConfiguration.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Countries.Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionAerolineas.src.Modules.Airlines.Infrastructure.Entity;

public sealed class AirlineEntityConfiguration : IEntityTypeConfiguration<AirlineEntity>
{
    public void Configure(EntityTypeBuilder<AirlineEntity> builder)
    {
        builder.ToTable("airlines");

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
            .Property(x => x.OriginCountryId)
            .HasColumnName("pais_origen_id")
            .HasColumnType("int")
            .IsRequired();
        builder
            .HasOne<CountryEntity>()
            .WithMany()
            .HasForeignKey(x => x.OriginCountryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .Property(x => x.IsActive)
            .HasColumnName("activa")
            .HasColumnType("tinyint(1)")
            .IsRequired();

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
    }
}

