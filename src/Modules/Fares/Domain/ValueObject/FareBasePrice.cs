// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Fares\Domain\ValueObject\FareBasePrice.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Fares.Domain.ValueObject
{
    public sealed record FareBasePrice
    {
        public decimal Value { get; }

        private FareBasePrice(decimal value)
        {
            Value = value;
        }

        public static FareBasePrice Create(decimal value)
        {
            if (value < 0)
                throw new ArgumentException("El precio_base no puede ser negativo");

            return new FareBasePrice(value);
        }
    }
}

