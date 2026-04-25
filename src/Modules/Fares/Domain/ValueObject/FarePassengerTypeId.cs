// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Fares\Domain\ValueObject\FarePassengerTypeId.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Fares.Domain.ValueObject
{
    public sealed record FarePassengerTypeId
    {
        public int Value { get; }

        private FarePassengerTypeId(int value)
        {
            Value = value;
        }

        public static FarePassengerTypeId Create(int value)
        {
            if (value <= 0)
                throw new ArgumentException("El tipo_pasajero_id no puede ser menor a 1");

            return new FarePassengerTypeId(value);
        }
    }
}

