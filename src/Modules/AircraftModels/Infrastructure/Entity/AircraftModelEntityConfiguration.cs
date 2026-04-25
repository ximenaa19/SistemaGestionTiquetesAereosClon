// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\AircraftModels\Infrastructure\Entity\AircraftModelEntityConfiguration.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.AircraftManufacturers.Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionAerolineas.src.Modules.AircraftModels.Infrastructure.Entity;

public sealed class AircraftModelEntityConfiguration : IEntityTypeConfiguration<AircraftModelEntity>
{
    public void Configure(EntityTypeBuilder<AircraftModelEntity> builder)
    {
        builder.ToTable("aircraftmodels");

        builder.HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .HasColumnName("id")
            .HasColumnType("int")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder
            .Property(x => x.ManufacturerId)
            .HasColumnName("fabricante_id")
            .HasColumnType("int")
            .IsRequired();

        builder
            .Property(x => x.ModelName)
            .HasColumnName("nombre_modelo")
            .HasColumnType("varchar(100)")
            .IsRequired();

        builder
            .Property(x => x.MaxCapacity)
            .HasColumnName("capacidad_maxima")
            .HasColumnType("int")
            .IsRequired();

        builder
            .Property(x => x.MaxTakeoffWeightKg)
            .HasColumnName("peso_max_despegue_kg")
            .HasColumnType("decimal(10,2)");

        builder
            .Property(x => x.FuelConsumptionKgPerHour)
            .HasColumnName("consumo_combustible_kg_h")
            .HasColumnType("decimal(8,2)");

        builder
            .Property(x => x.CruiseSpeedKmh)
            .HasColumnName("velocidad_crucero_kmh")
            .HasColumnType("int");

        builder
            .Property(x => x.CruiseAltitudeFt)
            .HasColumnName("altitud_crucero_ft")
            .HasColumnType("int");

        builder
            .HasOne<AircraftManufacturerEntity>()
            .WithMany()
            .HasForeignKey(x => x.ManufacturerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

