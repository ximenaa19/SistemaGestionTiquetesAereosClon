// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Addresses\Infrastructure\Entity\AddressEntityConfiguration.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Cities.Infrastructure.Entity;
using GestionAerolineas.src.Modules.RoadTypes.Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionAerolineas.src.Modules.Addresses.Infrastructure.Entity;

public sealed class AddressEntityConfiguration : IEntityTypeConfiguration<AddressEntity>
{
    public void Configure(EntityTypeBuilder<AddressEntity> builder)
    {
        builder.ToTable("addresses");

        builder.HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .HasColumnName("id")
            .HasColumnType("int")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder
            .Property(x => x.RoadTypeId)
            .HasColumnName("tipo_via_id")
            .HasColumnType("int")
            .IsRequired();

        builder
            .Property(x => x.RoadName)
            .HasColumnName("nombre_via")
            .HasColumnType("varchar(100)")
            .IsRequired();

        builder
            .Property(x => x.Number)
            .HasColumnName("numero")
            .HasColumnType("varchar(20)");

        builder
            .Property(x => x.Complement)
            .HasColumnName("complemento")
            .HasColumnType("varchar(100)");

        builder
            .Property(x => x.CityId)
            .HasColumnName("ciudad_id")
            .HasColumnType("int")
            .IsRequired();

        builder
            .Property(x => x.PostalCode)
            .HasColumnName("codigo_postal")
            .HasColumnType("varchar(20)");

        builder
            .HasOne<RoadTypeEntity>()
            .WithMany()
            .HasForeignKey(x => x.RoadTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne<CityEntity>()
            .WithMany()
            .HasForeignKey(x => x.CityId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

