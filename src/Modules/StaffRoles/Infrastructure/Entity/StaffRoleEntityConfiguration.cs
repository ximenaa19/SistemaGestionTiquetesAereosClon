// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\StaffRoles\Infrastructure\Entity\StaffRoleEntityConfiguration.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionAerolineas.src.Modules.StaffRoles.Infrastructure.Entity;

public sealed class StaffRoleEntityConfiguration : IEntityTypeConfiguration<StaffRoleEntity>
{
    public void Configure(EntityTypeBuilder<StaffRoleEntity> builder)
    {
        builder.ToTable("cargos_personal");

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

        builder.HasIndex(x => x.Name).IsUnique();
    }
}
