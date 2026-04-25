// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Routes\Domain\ValueObject\RouteId.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Routes.Domain.ValueObject
{
    public sealed record RouteId
    {
        public int Value { get; }

        private RouteId(int value)
        {
            Value = value;
        }

        public static RouteId Create(int value)
        {
            if (value <= 0)
                throw new ArgumentException("El valor no puede ser menor a 1");

            return new RouteId(value);
        }

        public static RouteId CreateEmpty()
        {
            return new RouteId(0);
        }
    }
}

