// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Sessions\Infrastructure\Entity\SessionEntityConfiguration.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Users.Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionAerolineas.src.Modules.Sessions.Infrastructure.Entity;

public sealed class SessionEntityConfiguration : IEntityTypeConfiguration<SessionEntity>
{
    public void Configure(EntityTypeBuilder<SessionEntity> builder)
    {
        builder.ToTable("sessions");

        builder.HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .HasColumnName("id")
            .HasColumnType("int")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder
            .Property(x => x.UserId)
            .HasColumnName("usuario_id")
            .HasColumnType("int")
            .IsRequired();

        builder
            .Property(x => x.StartedAt)
            .HasColumnName("iniciada_en")
            .HasColumnType("datetime")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();

        builder
            .Property(x => x.EndedAt)
            .HasColumnName("cerrada_en")
            .HasColumnType("datetime");

        builder
            .Property(x => x.IpAddress)
            .HasColumnName("ip_origen")
            .HasColumnType("varchar(45)");

        builder
            .Property(x => x.IsActive)
            .HasColumnName("activa")
            .HasColumnType("tinyint(1)")
            .HasDefaultValue(true)
            .IsRequired();

        builder
            .HasOne<UserEntity>()
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
