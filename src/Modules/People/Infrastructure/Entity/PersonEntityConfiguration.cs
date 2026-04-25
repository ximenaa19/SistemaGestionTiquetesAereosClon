// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\People\Infrastructure\Entity\PersonEntityConfiguration.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Addresses.Infrastructure.Entity;
using GestionAerolineas.src.Modules.DocumentTypes.Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionAerolineas.src.Modules.People.Infrastructure.Entity;

public sealed class PersonEntityConfiguration : IEntityTypeConfiguration<PersonEntity>
{
    public void Configure(EntityTypeBuilder<PersonEntity> builder)
    {
        builder.ToTable("people");

        builder.HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .HasColumnName("id")
            .HasColumnType("int")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder
            .Property(x => x.DocumentTypeId)
            .HasColumnName("tipo_documento_id")
            .HasColumnType("int")
            .IsRequired();

        builder
            .Property(x => x.DocumentNumber)
            .HasColumnName("numero_documento")
            .HasColumnType("varchar(30)")
            .IsRequired();

        builder
            .Property(x => x.FirstNames)
            .HasColumnName("nombres")
            .HasColumnType("varchar(100)")
            .IsRequired();

        builder
            .Property(x => x.LastNames)
            .HasColumnName("apellidos")
            .HasColumnType("varchar(100)")
            .IsRequired();

        builder
            .Property(x => x.BirthDate)
            .HasColumnName("fecha_nacimiento")
            .HasColumnType("date");

        builder
            .Property(x => x.Gender)
            .HasColumnName("genero")
            .HasColumnType("char(1)");

        builder
            .Property(x => x.AddressId)
            .HasColumnName("direccion_id")
            .HasColumnType("int");

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

        builder
            .HasIndex(x => new { x.DocumentTypeId, x.DocumentNumber })
            .IsUnique();

        builder
            .HasOne<DocumentTypeEntity>()
            .WithMany()
            .HasForeignKey(x => x.DocumentTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne<AddressEntity>()
            .WithMany()
            .HasForeignKey(x => x.AddressId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

