// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\SystemRoles\Infrastructure\Entity\SystemRoleEntityConfiguration.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionAerolineas.src.Modules.SystemRoles.Infrastructure.Entity;

public sealed class SystemRoleEntityConfiguration : IEntityTypeConfiguration<SystemRoleEntity>
{
    public void Configure(EntityTypeBuilder<SystemRoleEntity> builder)
    {
        builder.ToTable("roles_sistema");

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
            .HasColumnType("varchar(50)")
            .IsRequired();

        builder
            .Property(x => x.Description)
            .HasColumnName("descripcion")
            .HasColumnType("varchar(150)");

        builder.HasIndex(x => x.Name).IsUnique();
    }
}
