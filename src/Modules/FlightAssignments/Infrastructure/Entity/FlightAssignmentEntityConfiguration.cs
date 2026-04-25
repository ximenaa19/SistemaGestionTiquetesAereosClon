// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\FlightAssignments\Infrastructure\Entity\FlightAssignmentEntityConfiguration.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.FlightAssignments.Infrastructure.Entity;
using GestionAerolineas.src.Modules.FlightRoles.Infrastructure.Entity;
using GestionAerolineas.src.Modules.Flights.Infrastructure.Entity;
using GestionAerolineas.src.Modules.Staff.Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionAerolineas.src.Modules.FlightAssignments.Infrastructure.Entity;

public sealed class FlightAssignmentEntityConfiguration : IEntityTypeConfiguration<FlightAssignmentEntity>
{
    public void Configure(EntityTypeBuilder<FlightAssignmentEntity> builder)
    {
        builder.ToTable("flightassignments");

        builder.HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .HasColumnName("id")
            .HasColumnType("int")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder
            .Property(x => x.FlightId)
            .HasColumnName("vuelo_id")
            .HasColumnType("int")
            .IsRequired();

        builder
            .Property(x => x.StaffId)
            .HasColumnName("personal_id")
            .HasColumnType("int")
            .IsRequired();

        builder
            .Property(x => x.FlightRoleId)
            .HasColumnName("rol_vuelo_id")
            .HasColumnType("int")
            .IsRequired();

        builder
            .HasIndex(x => new { x.FlightId, x.StaffId })
            .IsUnique();

        builder
            .HasOne<FlightEntity>()
            .WithMany()
            .HasForeignKey(x => x.FlightId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne<StaffEntity>()
            .WithMany()
            .HasForeignKey(x => x.StaffId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne<FlightRoleEntity>()
            .WithMany()
            .HasForeignKey(x => x.FlightRoleId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

