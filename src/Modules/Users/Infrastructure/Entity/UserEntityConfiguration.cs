// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Users\Infrastructure\Entity\UserEntityConfiguration.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.People.Infrastructure.Entity;
using GestionAerolineas.src.Modules.SystemRoles.Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionAerolineas.src.Modules.Users.Infrastructure.Entity;

public sealed class UserEntityConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.ToTable("users");

        builder.HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .HasColumnName("id")
            .HasColumnType("int")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder
            .Property(x => x.Username)
            .HasColumnName("username")
            .HasColumnType("varchar(50)")
            .IsRequired();

        builder
            .Property(x => x.PasswordHash)
            .HasColumnName("password_hash")
            .HasColumnType("varchar(255)")
            .IsRequired();

        builder
            .Property(x => x.PersonId)
            .HasColumnName("persona_id")
            .HasColumnType("int");

        builder
            .Property(x => x.RoleId)
            .HasColumnName("rol_id")
            .HasColumnType("int")
            .IsRequired();

        builder
            .Property(x => x.IsActive)
            .HasColumnName("activo")
            .HasColumnType("tinyint(1)")
            .HasDefaultValue(true)
            .IsRequired();

        builder
            .Property(x => x.LastAccess)
            .HasColumnName("ultimo_acceso")
            .HasColumnType("datetime");

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

        builder.HasIndex(x => x.Username).IsUnique();
        builder.HasIndex(x => x.PersonId).IsUnique();

        builder
            .HasOne<PersonEntity>()
            .WithMany()
            .HasForeignKey(x => x.PersonId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne<SystemRoleEntity>()
            .WithMany()
            .HasForeignKey(x => x.RoleId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
