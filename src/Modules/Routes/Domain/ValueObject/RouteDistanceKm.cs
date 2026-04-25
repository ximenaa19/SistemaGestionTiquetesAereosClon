// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Routes\Domain\ValueObject\RouteDistanceKm.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Routes.Domain.ValueObject
{
    public sealed record RouteDistanceKm
    {
        public int? Value { get; }

        private RouteDistanceKm(int? value)
        {
            Value = value;
        }

        public static RouteDistanceKm Create(int? value)
        {
            if (!value.HasValue)
                return new RouteDistanceKm((int?)null);

            if (value.Value < 0)
                throw new ArgumentException("La distancia_km no puede ser negativa");

            return new RouteDistanceKm(value.Value);
        }
    }
}

