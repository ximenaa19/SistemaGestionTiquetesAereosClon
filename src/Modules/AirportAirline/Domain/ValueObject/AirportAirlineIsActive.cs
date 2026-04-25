// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\AirportAirline\Domain\ValueObject\AirportAirlineIsActive.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.AirportAirline.Domain.ValueObject
{
    public sealed record AirportAirlineIsActive
    {
        public bool Value { get; }

        private AirportAirlineIsActive(bool value)
        {
            Value = value;
        }

        public static AirportAirlineIsActive Create(bool value)
        {
            return new AirportAirlineIsActive(value);
        }
    }
}

