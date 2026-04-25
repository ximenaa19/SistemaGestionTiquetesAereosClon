// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Tickets\Infrastructure\Entity\TicketEntityConfiguration.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.ReservationPassengers.Infrastructure.Entity;
using GestionAerolineas.src.Modules.TicketStatuses.Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionAerolineas.src.Modules.Tickets.Infrastructure.Entity;

public sealed class TicketEntityConfiguration : IEntityTypeConfiguration<TicketEntity>
{
    public void Configure(EntityTypeBuilder<TicketEntity> builder)
    {
        builder.ToTable("tickets");

        builder.HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .HasColumnName("id")
            .HasColumnType("int")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder
            .Property(x => x.ReservationPassengerId)
            .HasColumnName("reserva_pasajero_id")
            .HasColumnType("int")
            .IsRequired();

        builder
            .Property(x => x.Code)
            .HasColumnName("codigo_tiquete")
            .HasColumnType("varchar(30)")
            .IsRequired();

        builder
            .Property(x => x.IssuedAt)
            .HasColumnName("fecha_emision")
            .HasColumnType("datetime")
            .IsRequired();

        builder
            .Property(x => x.StatusId)
            .HasColumnName("estado_tiquete_id")
            .HasColumnType("int")
            .IsRequired();

        builder
            .Property(x => x.CreatedAt)
            .HasColumnName("creado_en")
            .HasColumnType("datetime")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd();

        builder
            .Property(x => x.UpdatedAt)
            .HasColumnName("actualizado_en")
            .HasColumnType("datetime")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAddOrUpdate();

        builder.HasIndex(x => x.ReservationPassengerId).IsUnique();
        builder.HasIndex(x => x.Code).IsUnique();

        builder
            .HasOne<ReservationPassengerEntity>()
            .WithMany()
            .HasForeignKey(x => x.ReservationPassengerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne<TicketStatusEntity>()
            .WithMany()
            .HasForeignKey(x => x.StatusId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

