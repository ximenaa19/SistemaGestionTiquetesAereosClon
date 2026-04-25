// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\People\Domain\ValueObject\PersonAddressId.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.People.Domain.ValueObject
{
    public sealed record PersonAddressId
    {
        public int? Value { get; }

        private PersonAddressId(int? value)
        {
            Value = value;
        }

        public static PersonAddressId Create(int? value)
        {
            if (!value.HasValue)
                return new PersonAddressId((int?)null);

            if (value.Value <= 0)
                throw new ArgumentException("El valor no puede ser menor a 1");

            return new PersonAddressId(value.Value);
        }
    }
}
