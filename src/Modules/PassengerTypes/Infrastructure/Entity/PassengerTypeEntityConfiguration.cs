// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\PassengerTypes\Infrastructure\Entity\PassengerTypeEntityConfiguration.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionAerolineas.src.Modules.PassengerTypes.Infrastructure.Entity;

public sealed class PassengerTypeEntityConfiguration : IEntityTypeConfiguration<PassengerTypeEntity>
{
    public void Configure(EntityTypeBuilder<PassengerTypeEntity> builder)
    {
        builder.ToTable("tipos_pasajero");

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

        builder
            .Property(x => x.AgeMin)
            .HasColumnName("edad_min")
            .HasColumnType("int");

        builder
            .Property(x => x.AgeMax)
            .HasColumnName("edad_max")
            .HasColumnType("int");

        builder.HasIndex(x => x.Name).IsUnique();
    }
}

