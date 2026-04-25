// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Passengers\Infrastructure\Entity\PassengerEntityConfiguration.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.PassengerTypes.Infrastructure.Entity;
using GestionAerolineas.src.Modules.People.Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionAerolineas.src.Modules.Passengers.Infrastructure.Entity;

public sealed class PassengerEntityConfiguration : IEntityTypeConfiguration<PassengerEntity>
{
    public void Configure(EntityTypeBuilder<PassengerEntity> builder)
    {
        builder.ToTable("passengers");

        builder.HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .HasColumnName("id")
            .HasColumnType("int")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder
            .Property(x => x.PersonId)
            .HasColumnName("persona_id")
            .HasColumnType("int")
            .IsRequired();

        builder
            .Property(x => x.PassengerTypeId)
            .HasColumnName("tipo_pasajero_id")
            .HasColumnType("int")
            .IsRequired();

        builder.HasIndex(x => x.PersonId).IsUnique();

        builder
            .HasOne<PersonEntity>()
            .WithMany()
            .HasForeignKey(x => x.PersonId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne<PassengerTypeEntity>()
            .WithMany()
            .HasForeignKey(x => x.PassengerTypeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
