// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\FlightStates\Infrastructure\Entity\FlightStateEntityConfiguration.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionAerolineas.src.Modules.FlightStates.Infrastructure.Entity;

public sealed class FlightStateEntityConfiguration : IEntityTypeConfiguration<FlightStateEntity>
{
    public void Configure(EntityTypeBuilder<FlightStateEntity> builder)
    {
        builder.ToTable("estados_vuelo");

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
