// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Reservations\Domain\ValueObject\ReservationCustomerId.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Reservations.Domain.ValueObject
{
    public sealed record ReservationCustomerId
    {
        public int Value { get; }

        private ReservationCustomerId(int value)
        {
            Value = value;
        }

        public static ReservationCustomerId Create(int value)
        {
            if (value <= 0)
                throw new ArgumentException("El cliente_id no puede ser menor a 1");

            return new ReservationCustomerId(value);
        }
    }
}

