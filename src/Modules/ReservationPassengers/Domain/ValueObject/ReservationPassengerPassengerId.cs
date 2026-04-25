// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\ReservationPassengers\Domain\ValueObject\ReservationPassengerPassengerId.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.ReservationPassengers.Domain.ValueObject
{
    public sealed record ReservationPassengerPassengerId
    {
        public int Value { get; }

        private ReservationPassengerPassengerId(int value)
        {
            Value = value;
        }

        public static ReservationPassengerPassengerId Create(int value)
        {
            if (value <= 0)
                throw new ArgumentException("El pasajero_id no puede ser menor a 1");

            return new ReservationPassengerPassengerId(value);
        }
    }
}

