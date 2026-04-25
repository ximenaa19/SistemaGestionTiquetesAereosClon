// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\AirportAirline\Domain\ValueObject\AirportAirlineEndDate.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.AirportAirline.Domain.ValueObject
{
    public sealed record AirportAirlineEndDate
    {
        public DateTime? Value { get; }

        private AirportAirlineEndDate(DateTime? value)
        {
            Value = value?.Date;
        }

        public static AirportAirlineEndDate Create(DateTime? value)
        {
            return new AirportAirlineEndDate(value);
        }
    }
}

