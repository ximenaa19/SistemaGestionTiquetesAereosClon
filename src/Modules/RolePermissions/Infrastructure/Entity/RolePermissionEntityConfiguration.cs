// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\RolePermissions\Infrastructure\Entity\RolePermissionEntityConfiguration.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Permissions.Infrastructure.Entity;
using GestionAerolineas.src.Modules.SystemRoles.Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionAerolineas.src.Modules.RolePermissions.Infrastructure.Entity;

public sealed class RolePermissionEntityConfiguration : IEntityTypeConfiguration<RolePermissionEntity>
{
    public void Configure(EntityTypeBuilder<RolePermissionEntity> builder)
    {
        builder.ToTable("rolepermissions");

        builder.HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .HasColumnName("id")
            .HasColumnType("int")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder
            .Property(x => x.RoleId)
            .HasColumnName("rol_id")
            .HasColumnType("int")
            .IsRequired();

        builder
            .Property(x => x.PermissionId)
            .HasColumnName("permiso_id")
            .HasColumnType("int")
            .IsRequired();

        builder.HasIndex(x => new { x.RoleId, x.PermissionId }).IsUnique();

        builder
            .HasOne<SystemRoleEntity>()
            .WithMany()
            .HasForeignKey(x => x.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne<PermissionEntity>()
            .WithMany()
            .HasForeignKey(x => x.PermissionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

