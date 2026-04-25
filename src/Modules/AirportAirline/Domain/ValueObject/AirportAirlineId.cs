// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\AirportAirline\Domain\ValueObject\AirportAirlineId.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.AirportAirline.Domain.ValueObject
{
    public sealed record AirportAirlineId
    {
        public int Value { get; }

        private AirportAirlineId(int value)
        {
            Value = value;
        }

        public static AirportAirlineId Create(int value)
        {
            if (value <= 0)
                throw new ArgumentException("El valor no puede ser menor a 1");

            return new AirportAirlineId(value);
        }

        public static AirportAirlineId CreateEmpty()
        {
            return new AirportAirlineId(0);
        }
    }
}

