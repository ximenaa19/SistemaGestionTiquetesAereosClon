// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\RouteStops\Domain\ValueObject\RouteStopDurationMinutes.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.RouteStops.Domain.ValueObject
{
    public sealed record RouteStopDurationMinutes
    {
        public int Value { get; }

        private RouteStopDurationMinutes(int value)
        {
            Value = value;
        }

        public static RouteStopDurationMinutes Create(int value)
        {
            if (value < 0)
                throw new ArgumentException("La duracion_escala_min no puede ser negativa");

            return new RouteStopDurationMinutes(value);
        }
    }
}

