// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Aircraft\Domain\ValueObject\AircraftId.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Aircraft.Domain.ValueObject
{
    public sealed record AircraftId
    {
        public int Value { get; }

        private AircraftId(int value)
        {
            Value = value;
        }

        public static AircraftId Create(int value)
        {
            if (value <= 0)
                throw new ArgumentException("El valor no puede ser menor a 1");

            return new AircraftId(value);
        }

        public static AircraftId CreateEmpty()
        {
            return new AircraftId(0);
        }
    }
}

