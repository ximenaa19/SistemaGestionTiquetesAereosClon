// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\CheckinStatuses\Infrastructure\Entity\CheckinStatusEntityConfiguration.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionAerolineas.src.Modules.CheckinStatuses.Infrastructure.Entity;

public sealed class CheckinStatusEntityConfiguration : IEntityTypeConfiguration<CheckinStatusEntity>
{
    public void Configure(EntityTypeBuilder<CheckinStatusEntity> builder)
    {
        builder.ToTable("estados_checkin");

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
