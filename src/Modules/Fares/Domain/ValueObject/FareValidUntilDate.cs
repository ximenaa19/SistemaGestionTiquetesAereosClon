// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Fares\Domain\ValueObject\FareValidUntilDate.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Fares.Domain.ValueObject
{
    public sealed record FareValidUntilDate
    {
        public DateTime? Value { get; }

        private FareValidUntilDate(DateTime? value)
        {
            Value = value?.Date;
        }

        public static FareValidUntilDate Create(DateTime? value)
        {
            return new FareValidUntilDate(value);
        }
    }
}

