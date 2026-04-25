// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Reservations\Domain\ValueObject\ReservationStatusId.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Reservations.Domain.ValueObject
{
    public sealed record ReservationStatusId
    {
        public int Value { get; }

        private ReservationStatusId(int value)
        {
            Value = value;
        }

        public static ReservationStatusId Create(int value)
        {
            if (value <= 0)
                throw new ArgumentException("El estado_reserva_id no puede ser menor a 1");

            return new ReservationStatusId(value);
        }
    }
}

