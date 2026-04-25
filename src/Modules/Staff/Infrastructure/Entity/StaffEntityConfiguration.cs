// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Staff\Infrastructure\Entity\StaffEntityConfiguration.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Airlines.Infrastructure.Entity;
using GestionAerolineas.src.Modules.Airports.Infrastructure.Entity;
using GestionAerolineas.src.Modules.People.Infrastructure.Entity;
using GestionAerolineas.src.Modules.StaffRoles.Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionAerolineas.src.Modules.Staff.Infrastructure.Entity;

public sealed class StaffEntityConfiguration : IEntityTypeConfiguration<StaffEntity>
{
    public void Configure(EntityTypeBuilder<StaffEntity> builder)
    {
        builder.ToTable("staff");

        builder.HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .HasColumnName("id")
            .HasColumnType("int")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder
            .Property(x => x.PersonId)
            .HasColumnName("persona_id")
            .HasColumnType("int")
            .IsRequired();

        builder
            .Property(x => x.RoleId)
            .HasColumnName("cargo_id")
            .HasColumnType("int")
            .IsRequired();

        builder
            .Property(x => x.AirlineId)
            .HasColumnName("aerolinea_id")
            .HasColumnType("int");

        builder
            .Property(x => x.AirportId)
            .HasColumnName("aeropuerto_id")
            .HasColumnType("int");

        builder
            .Property(x => x.HireDate)
            .HasColumnName("fecha_ingreso")
            .HasColumnType("date")
            .IsRequired();

        builder
            .Property(x => x.IsActive)
            .HasColumnName("activo")
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

        builder.HasIndex(x => x.PersonId).IsUnique();

        builder
            .HasOne<PersonEntity>()
            .WithMany()
            .HasForeignKey(x => x.PersonId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne<StaffRoleEntity>()
            .WithMany()
            .HasForeignKey(x => x.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne<AirlineEntity>()
            .WithMany()
            .HasForeignKey(x => x.AirlineId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne<AirportEntity>()
            .WithMany()
            .HasForeignKey(x => x.AirportId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

