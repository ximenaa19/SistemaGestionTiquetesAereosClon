// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\RoadTypes\Infrastructure\Entity\RoadTypeEntityConfiguration.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionAerolineas.src.Modules.RoadTypes.Infrastructure.Entity;

public sealed class RoadTypeEntityConfiguration : IEntityTypeConfiguration<RoadTypeEntity>
{
    public void Configure(EntityTypeBuilder<RoadTypeEntity> builder)
    {
        builder.ToTable("RoadTypes");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedNever();
        builder.Property(e => e.Name).IsRequired();
    }
    
}




