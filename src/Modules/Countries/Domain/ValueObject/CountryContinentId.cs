// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Countries\Domain\ValueObject\CountryContinentId.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Countries.Domain.ValueObject;

public class CountryContinentId
{
    public int Value { get; }

    public CountryContinentId(int value)
    {
        Value = value;
    }

    public static CountryContinentId Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("El valor no puede ser menor a 1");

        return new CountryContinentId(value);
    }
}


