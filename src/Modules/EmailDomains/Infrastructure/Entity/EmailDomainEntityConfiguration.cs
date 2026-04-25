// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\EmailDomains\Infrastructure\Entity\EmailDomainEntityConfiguration.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionAerolineas.src.Modules.EmailDomains.Infrastructure.Entity;

public sealed class EmailDomainEntityConfiguration : IEntityTypeConfiguration<EmailDomainEntity>
{
    public void Configure(EntityTypeBuilder<EmailDomainEntity> builder)
    {
        builder.ToTable("emaildomains");

        builder.HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .HasColumnName("id")
            .HasColumnType("int")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder
            .Property(x => x.Domain)
            .HasColumnName("domain")
            .HasColumnType("varchar(100)")
            .IsRequired();

        builder.HasIndex(x => x.Domain).IsUnique();
    }
}

