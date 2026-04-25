// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\AvailabilityStatuses\Infrastructure\Entity\AvailabilityStatusEntityConfiguration.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionAerolineas.src.Modules.AvailabilityStatuses.Infrastructure.Entity;

public sealed class AvailabilityStatusEntityConfiguration : IEntityTypeConfiguration<AvailabilityStatusEntity>
{
    public void Configure(EntityTypeBuilder<AvailabilityStatusEntity> builder)
    {
        builder.ToTable("estados_disponibilidad");

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

        builder.HasIndex(x => x.Name).IsUnique();
    }
}
