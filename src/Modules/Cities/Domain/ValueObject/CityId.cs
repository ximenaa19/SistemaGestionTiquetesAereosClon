// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Cities\Domain\ValueObject\CityId.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Cities.Domain.ValueObject
{
    public sealed record CityId
    {
        public int Value { get; }

        private CityId(int value)
        {
            Value = value;
        }

        public static CityId Create(int value)
        {
            if (value <= 0)
            {
                throw new ArgumentException("El valor no puede ser menor a 1");
            }

            return new CityId(value);
        }

        public static CityId CreateEmpty()
        {
            return new CityId(0);
        }
    }
}


