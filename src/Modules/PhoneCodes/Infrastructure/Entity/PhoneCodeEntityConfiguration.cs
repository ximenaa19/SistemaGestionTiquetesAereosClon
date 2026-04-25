// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\PhoneCodes\Infrastructure\Entity\PhoneCodeEntityConfiguration.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionAerolineas.src.Modules.PhoneCodes.Infrastructure.Entity;

public sealed class PhoneCodeEntityConfiguration : IEntityTypeConfiguration<PhoneCodeEntity>
{
    public void Configure(EntityTypeBuilder<PhoneCodeEntity> builder)
    {
        builder.ToTable("phonecodes");

        builder.HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .HasColumnName("id")
            .HasColumnType("int")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder
            .Property(x => x.CountryCode)
            .HasColumnName("codigo_pais")
            .HasColumnType("varchar(5)")
            .IsRequired();

        builder
            .Property(x => x.CountryName)
            .HasColumnName("nombre_pais")
            .HasColumnType("varchar(100)")
            .IsRequired();

        builder.HasIndex(x => x.CountryCode).IsUnique();
    }
}

