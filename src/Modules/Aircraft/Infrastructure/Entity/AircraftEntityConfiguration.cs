// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Aircraft\Infrastructure\Entity\AircraftEntityConfiguration.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.AircraftModels.Infrastructure.Entity;
using GestionAerolineas.src.Modules.Airlines.Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionAerolineas.src.Modules.Aircraft.Infrastructure.Entity;

public sealed class AircraftEntityConfiguration : IEntityTypeConfiguration<AircraftEntity>
{
    public void Configure(EntityTypeBuilder<AircraftEntity> builder)
    {
        builder.ToTable("aircraft");

        builder.HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .HasColumnName("id")
            .HasColumnType("int")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder
            .Property(x => x.ModelId)
            .HasColumnName("modelo_id")
            .HasColumnType("int")
            .IsRequired();

        builder
            .Property(x => x.AirlineId)
            .HasColumnName("aerolinea_id")
            .HasColumnType("int")
            .IsRequired();

        builder
            .Property(x => x.Registration)
            .HasColumnName("matricula")
            .HasColumnType("varchar(20)")
            .IsRequired();

        builder
            .Property(x => x.ManufactureDate)
            .HasColumnName("fecha_fabricacion")
            .HasColumnType("date");

        builder
            .Property(x => x.IsActive)
            .HasColumnName("activa")
            .HasColumnType("tinyint(1)")
            .IsRequired();

        builder.HasIndex(x => x.Registration).IsUnique();

        builder
            .HasOne<AircraftModelEntity>()
            .WithMany()
            .HasForeignKey(x => x.ModelId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne<AirlineEntity>()
            .WithMany()
            .HasForeignKey(x => x.AirlineId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

