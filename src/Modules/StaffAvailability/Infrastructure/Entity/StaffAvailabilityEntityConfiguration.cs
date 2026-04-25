// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\StaffAvailability\Infrastructure\Entity\StaffAvailabilityEntityConfiguration.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.AvailabilityStatuses.Infrastructure.Entity;
using GestionAerolineas.src.Modules.Staff.Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionAerolineas.src.Modules.StaffAvailability.Infrastructure.Entity;

public sealed class StaffAvailabilityEntityConfiguration : IEntityTypeConfiguration<StaffAvailabilityEntity>
{
    public void Configure(EntityTypeBuilder<StaffAvailabilityEntity> builder)
    {
        builder.ToTable("staffavailability");

        builder.HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .HasColumnName("id")
            .HasColumnType("int")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder
            .Property(x => x.StaffId)
            .HasColumnName("personal_id")
            .HasColumnType("int")
            .IsRequired();

        builder
            .Property(x => x.AvailabilityStatusId)
            .HasColumnName("estado_disponibilidad_id")
            .HasColumnType("int")
            .IsRequired();

        builder
            .Property(x => x.StartDateTime)
            .HasColumnName("fecha_inicio")
            .HasColumnType("datetime")
            .IsRequired();

        builder
            .Property(x => x.EndDateTime)
            .HasColumnName("fecha_fin")
            .HasColumnType("datetime")
            .IsRequired();

        builder
            .Property(x => x.Observation)
            .HasColumnName("observacion")
            .HasColumnType("varchar(255)");

        builder
            .HasOne<StaffEntity>()
            .WithMany()
            .HasForeignKey(x => x.StaffId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne<AvailabilityStatusEntity>()
            .WithMany()
            .HasForeignKey(x => x.AvailabilityStatusId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

