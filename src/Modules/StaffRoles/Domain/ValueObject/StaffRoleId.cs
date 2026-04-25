// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\StaffRoles\Domain\ValueObject\StaffRoleId.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.StaffRoles.Domain.ValueObject;

public sealed record StaffRoleId
{
    public int Value { get; }

    private StaffRoleId(int value)
    {
        Value = value;
    }

    public static StaffRoleId Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("El valor no puede ser menor a 1");

        return new StaffRoleId(value);
    }

    public static StaffRoleId CreateEmpty()
    {
        return new StaffRoleId(0);
    }
}
