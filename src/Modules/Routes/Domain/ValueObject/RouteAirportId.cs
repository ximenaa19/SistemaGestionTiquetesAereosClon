// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Routes\Domain\ValueObject\RouteAirportId.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Routes.Domain.ValueObject
{
    public sealed record RouteAirportId
    {
        public int Value { get; }

        private RouteAirportId(int value)
        {
            Value = value;
        }

        public static RouteAirportId Create(int value)
        {
            if (value <= 0)
                throw new ArgumentException("El valor no puede ser menor a 1");

            return new RouteAirportId(value);
        }
    }
}

