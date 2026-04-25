// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\PhoneCodes\Infrastructure\Entity\PhoneCodeEntity.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.PhoneCodes.Infrastructure.Entity;

public class PhoneCodeEntity
{
    public int Id { get; set; }
    public string? CountryCode { get; set; }
    public string? CountryName { get; set; }
}

