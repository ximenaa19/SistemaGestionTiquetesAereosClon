// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Airlines\Domain\ValueObject\AirlineIsActive.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Airlines.Domain.ValueObject
{
    public sealed record AirlineIsActive
    {
        public bool Value { get; }

        private AirlineIsActive(bool value)
        {
            Value = value;
        }

        public static AirlineIsActive Create(bool value)
        {
            return new AirlineIsActive(value);
        }
    }
}

