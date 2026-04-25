// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Checkins\Infrastructure\Entity\CheckinEntityConfiguration.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.CheckinStatuses.Infrastructure.Entity;
using GestionAerolineas.src.Modules.FlightSeats.Infrastructure.Entity;
using GestionAerolineas.src.Modules.Staff.Infrastructure.Entity;
using GestionAerolineas.src.Modules.Tickets.Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionAerolineas.src.Modules.Checkins.Infrastructure.Entity;

public sealed class CheckinEntityConfiguration : IEntityTypeConfiguration<CheckinEntity>
{
    public void Configure(EntityTypeBuilder<CheckinEntity> builder)
    {
        builder.ToTable("checkins");

        builder.HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .HasColumnName("id")
            .HasColumnType("int")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder
            .Property(x => x.TicketId)
            .HasColumnName("tiquete_id")
            .HasColumnType("int")
            .IsRequired();

        builder
            .Property(x => x.StaffId)
            .HasColumnName("personal_id")
            .HasColumnType("int")
            .IsRequired();

        builder
            .Property(x => x.FlightSeatId)
            .HasColumnName("asiento_vuelo_id")
            .HasColumnType("int")
            .IsRequired();

        builder
            .Property(x => x.CheckedAt)
            .HasColumnName("fecha_checkin")
            .HasColumnType("datetime")
            .IsRequired();

        builder
            .Property(x => x.StatusId)
            .HasColumnName("estado_checkin_id")
            .HasColumnType("int")
            .IsRequired();

        builder
            .Property(x => x.BoardingPassNumber)
            .HasColumnName("numero_tarjeta_embarque")
            .HasColumnType("varchar(20)")
            .IsRequired();

        builder
            .Property(x => x.HasHoldBaggage)
            .HasColumnName("equipaje_bodega")
            .HasColumnType("tinyint(1)")
            .HasDefaultValue(false)
            .IsRequired();

        builder
            .Property(x => x.BaggageWeightKg)
            .HasColumnName("peso_equipaje_kg")
            .HasColumnType("decimal(5,2)")
            .HasDefaultValue(0m);

        builder.HasIndex(x => x.TicketId).IsUnique();
        builder.HasIndex(x => x.FlightSeatId).IsUnique();
        builder.HasIndex(x => x.BoardingPassNumber).IsUnique();

        builder
            .HasOne<TicketEntity>()
            .WithMany()
            .HasForeignKey(x => x.TicketId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne<StaffEntity>()
            .WithMany()
            .HasForeignKey(x => x.StaffId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne<FlightSeatEntity>()
            .WithMany()
            .HasForeignKey(x => x.FlightSeatId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne<CheckinStatusEntity>()
            .WithMany()
            .HasForeignKey(x => x.StatusId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

