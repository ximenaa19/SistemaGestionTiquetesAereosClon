// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Continents\Infrastructure\Entity\ContinentEntityConfiguration.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionAerolineas.src.Modules.Continents.Infrastructure.Entity;

public class ContinentEntityConfiguration : IEntityTypeConfiguration<ContinentEntity>
{
    public void Configure(EntityTypeBuilder<ContinentEntity> builder)
    {
        builder.ToTable("continents");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).ValueGeneratedNever();

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(e => e.Name).IsUnique();
    }
}