// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\People\Infrastructure\Repository\PersonRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.People.Domain.Aggregate;
using GestionAerolineas.src.Modules.People.Domain.Repositories;
using GestionAerolineas.src.Modules.People.Domain.ValueObject;
using GestionAerolineas.src.Modules.People.Infrastructure.Entity;
using GestionAerolineas.src.shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestionAerolineas.src.Modules.People.Infrastructure.Repository;

public class PersonRepository : IPersonRepository
{
    private readonly AppDbContext _context;

    public PersonRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Person>> GetAllAsync()
    {
        var entities = await _context.People.AsNoTracking().ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<Person?> GetByIdAsync(PersonId id)
    {
        var entity = await _context.People
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<Person?> GetByDocumentAsync(PersonDocumentTypeId documentTypeId, PersonDocumentNumber documentNumber)
    {
        var entity = await _context.People
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.DocumentTypeId == documentTypeId.Value && e.DocumentNumber == documentNumber.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task AddAsync(Person person)
    {
        await _context.People.AddAsync(MapToEntity(person));
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Person person)
    {
        var existing = await _context.People
            .FirstOrDefaultAsync(e => e.Id == person.Id.Value);

        if (existing is null)
            return;

        existing.DocumentTypeId = person.DocumentTypeId.Value;
        existing.DocumentNumber = person.DocumentNumber.Value;
        existing.FirstNames = person.FirstNames.Value;
        existing.LastNames = person.LastNames.Value;
        existing.BirthDate = person.BirthDate.Value;
        existing.Gender = person.Gender.Value;
        existing.AddressId = person.AddressId.Value;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Person person)
    {
        var entity = await _context.People.FindAsync(person.Id.Value);

        if (entity is null)
            return;

        _context.People.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public Task<bool> ExistsAsync(PersonId id)
    {
        return _context.People.AnyAsync(e => e.Id == id.Value);
    }

    public Task<bool> ExistsByNormalizedDocumentInTypeAsync(int documentTypeId, string normalizedDocumentNumber, int? excludingId = null)
    {
        var query = _context.People
            .AsNoTracking()
            .Where(p => p.DocumentTypeId == documentTypeId && p.DocumentNumber != null);

        if (excludingId.HasValue)
            query = query.Where(p => p.Id != excludingId.Value);

        return query.AnyAsync(p => p.DocumentNumber!.Trim().ToUpper() == normalizedDocumentNumber);
    }

    private static Person MapToDomain(PersonEntity entity)
    {
        try
        {
            return Person.Create(
                PersonId.Create(entity.Id),
                PersonDocumentTypeId.Create(entity.DocumentTypeId),
                PersonDocumentNumber.Create(entity.DocumentNumber ?? string.Empty),
                PersonFirstNames.Create(entity.FirstNames ?? string.Empty),
                PersonLastNames.Create(entity.LastNames ?? string.Empty),
                PersonBirthDate.Create(entity.BirthDate),
                PersonGender.Create(entity.Gender),
                PersonAddressId.Create(entity.AddressId)
            );
        }
        catch (Exception ex)
        {
            throw new Exception(
                $"El registro people(id={entity.Id}) tiene datos invalidos. " +
                $"tipo_documento_id={entity.DocumentTypeId}, numero_documento='{entity.DocumentNumber}', " +
                $"nombres='{entity.FirstNames}', apellidos='{entity.LastNames}', " +
                $"fecha_nacimiento='{entity.BirthDate}', genero='{entity.Gender}', direccion_id={entity.AddressId}.",
                ex);
        }
    }

    private static PersonEntity MapToEntity(Person person)
    {
        return new PersonEntity
        {
            Id = person.Id.Value,
            DocumentTypeId = person.DocumentTypeId.Value,
            DocumentNumber = person.DocumentNumber.Value,
            FirstNames = person.FirstNames.Value,
            LastNames = person.LastNames.Value,
            BirthDate = person.BirthDate.Value,
            Gender = person.Gender.Value,
            AddressId = person.AddressId.Value
        };
    }
}

