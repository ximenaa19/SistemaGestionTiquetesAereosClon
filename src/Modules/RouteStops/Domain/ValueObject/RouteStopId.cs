// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\RouteStops\Domain\ValueObject\RouteStopId.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.RouteStops.Domain.ValueObject
{
    public sealed record RouteStopId
    {
        public int Value { get; }

        private RouteStopId(int value)
        {
            Value = value;
        }

        public static RouteStopId Create(int value)
        {
            if (value <= 0)
                throw new ArgumentException("El valor no puede ser menor a 1");

            return new RouteStopId(value);
        }

        public static RouteStopId CreateEmpty()
        {
            return new RouteStopId(0);
        }
    }
}

