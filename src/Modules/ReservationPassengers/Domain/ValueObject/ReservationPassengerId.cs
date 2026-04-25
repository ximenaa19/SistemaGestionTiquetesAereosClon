// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\ReservationPassengers\Domain\ValueObject\ReservationPassengerId.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.ReservationPassengers.Domain.ValueObject
{
    public sealed record ReservationPassengerId
    {
        public int Value { get; }

        private ReservationPassengerId(int value)
        {
            Value = value;
        }

        public static ReservationPassengerId Create(int value)
        {
            if (value <= 0)
                throw new ArgumentException("El id no puede ser menor a 1");

            return new ReservationPassengerId(value);
        }

        public static ReservationPassengerId CreateEmpty()
        {
            return new ReservationPassengerId(0);
        }
    }
}

