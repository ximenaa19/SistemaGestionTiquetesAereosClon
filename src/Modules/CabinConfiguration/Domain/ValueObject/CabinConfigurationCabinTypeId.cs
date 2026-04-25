// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\CabinConfiguration\Domain\ValueObject\CabinConfigurationCabinTypeId.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.CabinConfiguration.Domain.ValueObject
{
    public sealed record CabinConfigurationCabinTypeId
    {
        public int Value { get; }

        private CabinConfigurationCabinTypeId(int value)
        {
            Value = value;
        }

        public static CabinConfigurationCabinTypeId Create(int value)
        {
            if (value <= 0)
                throw new ArgumentException("El valor no puede ser menor a 1");

            return new CabinConfigurationCabinTypeId(value);
        }
    }
}

