// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Routes\Domain\ValueObject\RouteEstimatedDurationMinutes.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Routes.Domain.ValueObject
{
    public sealed record RouteEstimatedDurationMinutes
    {
        public int? Value { get; }

        private RouteEstimatedDurationMinutes(int? value)
        {
            Value = value;
        }

        public static RouteEstimatedDurationMinutes Create(int? value)
        {
            if (!value.HasValue)
                return new RouteEstimatedDurationMinutes((int?)null);

            if (value.Value < 0)
                throw new ArgumentException("La duracion_estimada_min no puede ser negativa");

            return new RouteEstimatedDurationMinutes(value.Value);
        }
    }
}

