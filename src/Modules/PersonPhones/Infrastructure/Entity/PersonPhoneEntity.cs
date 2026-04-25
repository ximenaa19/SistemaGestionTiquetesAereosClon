// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\PersonPhones\Infrastructure\Entity\PersonPhoneEntity.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.PersonPhones.Infrastructure.Entity;

public class PersonPhoneEntity
{
    public int Id { get; set; }
    public int PersonId { get; set; }
    public int PhoneCodeId { get; set; }
    public string? PhoneNumber { get; set; }
    public bool IsPrimary { get; set; }
}

