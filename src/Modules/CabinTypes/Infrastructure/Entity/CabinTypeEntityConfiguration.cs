// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\CabinTypes\Infrastructure\Entity\CabinTypeEntityConfiguration.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionAerolineas.src.Modules.CabinTypes.Infrastructure.Entity;

public class CabinTypeEntityConfiguration : IEntityTypeConfiguration<CabinTypeEntity>
{
    public void Configure(EntityTypeBuilder<CabinTypeEntity> builder)
    {
        builder.ToTable("CabinTypes");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedNever();
        builder.Property(e => e.Name).IsRequired();
    }


}
