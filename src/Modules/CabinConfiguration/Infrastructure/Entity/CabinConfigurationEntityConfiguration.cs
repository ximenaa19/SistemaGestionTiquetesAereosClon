// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\CabinConfiguration\Infrastructure\Entity\CabinConfigurationEntityConfiguration.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Aircraft.Infrastructure.Entity;
using GestionAerolineas.src.Modules.CabinTypes.Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionAerolineas.src.Modules.CabinConfiguration.Infrastructure.Entity;

public sealed class CabinConfigurationEntityConfiguration : IEntityTypeConfiguration<CabinConfigurationEntity>
{
    public void Configure(EntityTypeBuilder<CabinConfigurationEntity> builder)
    {
        builder.ToTable("cabinconfiguration");

        builder.HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .HasColumnName("id")
            .HasColumnType("int")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder
            .Property(x => x.AircraftId)
            .HasColumnName("aeronave_id")
            .HasColumnType("int")
            .IsRequired();

        builder
            .Property(x => x.CabinTypeId)
            .HasColumnName("tipo_cabina_id")
            .HasColumnType("int")
            .IsRequired();

        builder
            .Property(x => x.StartRow)
            .HasColumnName("fila_inicio")
            .HasColumnType("int")
            .IsRequired();

        builder
            .Property(x => x.EndRow)
            .HasColumnName("fila_fin")
            .HasColumnType("int")
            .IsRequired();

        builder
            .Property(x => x.SeatsPerRow)
            .HasColumnName("asientos_por_fila")
            .HasColumnType("int")
            .IsRequired();

        builder
            .Property(x => x.SeatLetters)
            .HasColumnName("letras_asientos")
            .HasColumnType("varchar(10)")
            .IsRequired();

        builder
            .HasIndex(x => new { x.AircraftId, x.CabinTypeId })
            .IsUnique();

        builder
            .HasOne<AircraftEntity>()
            .WithMany()
            .HasForeignKey(x => x.AircraftId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne<CabinTypeEntity>()
            .WithMany()
            .HasForeignKey(x => x.CabinTypeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

