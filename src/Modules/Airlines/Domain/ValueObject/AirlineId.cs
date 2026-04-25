// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Airlines\Domain\ValueObject\AirlineId.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Airlines.Domain.ValueObject
{
    public sealed record AirlineId
    {
        public int Value { get; }

        private AirlineId(int value)
        {
            Value = value;
        }

        public static AirlineId Create(int value)
        {
            if (value <= 0)
            {
                throw new ArgumentException("El valor no puede ser menor a 1");
            }

            return new AirlineId(value);
        }

        public static AirlineId CreateEmpty()
        {
            return new AirlineId(0);
        }
    }
}

