// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Staff\Infrastructure\Repository\StaffRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.People.Infrastructure.Entity;
using GestionAerolineas.src.Modules.Staff.Domain.Aggregate;
using GestionAerolineas.src.Modules.Staff.Domain.Repositories;
using GestionAerolineas.src.Modules.Staff.Domain.ValueObject;
using GestionAerolineas.src.Modules.Staff.Infrastructure.Entity;
using GestionAerolineas.src.shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestionAerolineas.src.Modules.Staff.Infrastructure.Repository;

public class StaffRepository : IStaffRepository
{
    private readonly AppDbContext _context;

    public StaffRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<StaffMember>> GetAllAsync()
    {
        var entities = await _context.Staff
            .AsNoTracking()
            .OrderBy(e => e.Id)
            .ToListAsync();

        return entities.Select(MapToDomain).ToList();
    }

    public async Task<StaffMember?> GetByIdAsync(StaffId id)
    {
        var entity = await _context.Staff
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<StaffMember?> GetByPersonIdAsync(StaffPersonId personId)
    {
        var entity = await _context.Staff
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.PersonId == personId.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IEnumerable<StaffMember>> GetByRoleIdAsync(StaffRoleId roleId)
    {
        var entities = await _context.Staff
            .AsNoTracking()
            .Where(e => e.RoleId == roleId.Value)
            .OrderBy(e => e.Id)
            .ToListAsync();

        return entities.Select(MapToDomain).ToList();
    }

    public async Task<IEnumerable<StaffMember>> GetByIsActiveAsync(StaffIsActive isActive)
    {
        var entities = await _context.Staff
            .AsNoTracking()
            .Where(e => e.IsActive == isActive.Value)
            .OrderBy(e => e.Id)
            .ToListAsync();

        return entities.Select(MapToDomain).ToList();
    }

    public async Task<IEnumerable<StaffMember>> SearchByPersonNameOrLastNameAsync(string searchText)
    {
        var normalized = NormalizeSearch(searchText);
        if (string.IsNullOrWhiteSpace(normalized))
            return Array.Empty<StaffMember>();

        var query =
            from s in _context.Staff.AsNoTracking()
            join p in _context.People.AsNoTracking() on s.PersonId equals p.Id
            where p.FirstNames != null && p.LastNames != null
            let full = (p.FirstNames + " " + p.LastNames).Trim().ToUpper()
            where full.Contains(normalized)
            orderby s.Id
            select s;

        var entities = await query.ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task AddAsync(StaffMember staff)
    {
        await _context.Staff.AddAsync(MapToEntity(staff));
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(StaffMember staff)
    {
        var existing = await _context.Staff
            .FirstOrDefaultAsync(e => e.Id == staff.Id.Value);

        if (existing is null)
            return;

        existing.PersonId = staff.PersonId.Value;
        existing.RoleId = staff.RoleId.Value;
        existing.AirlineId = staff.AirlineId.Value;
        existing.AirportId = staff.AirportId.Value;
        existing.HireDate = staff.HireDate.Value;
        existing.IsActive = staff.IsActive.Value;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(StaffMember staff)
    {
        var entity = await _context.Staff.FindAsync(staff.Id.Value);
        if (entity is null)
            return;

        _context.Staff.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public Task<bool> ExistsAsync(StaffId id)
    {
        return _context.Staff.AnyAsync(e => e.Id == id.Value);
    }

    public Task<bool> ExistsByPersonIdAsync(int personId, int? excludingId = null)
    {
        var query = _context.Staff
            .AsNoTracking()
            .Where(s => s.PersonId == personId);

        if (excludingId.HasValue)
            query = query.Where(s => s.Id != excludingId.Value);

        return query.AnyAsync();
    }

    private static StaffMember MapToDomain(StaffEntity entity)
    {
        try
        {
            return StaffMember.Create(
                StaffId.Create(entity.Id),
                StaffPersonId.Create(entity.PersonId),
                StaffRoleId.Create(entity.RoleId),
                StaffAirlineId.Create(entity.AirlineId),
                StaffAirportId.Create(entity.AirportId),
                StaffHireDate.Create(entity.HireDate),
                StaffIsActive.Create(entity.IsActive)
            );
        }
        catch (Exception ex)
        {
            throw new Exception(
                $"El registro staff(id={entity.Id}) tiene datos invalidos. " +
                $"persona_id={entity.PersonId}, cargo_id={entity.RoleId}, aerolinea_id={entity.AirlineId}, " +
                $"aeropuerto_id={entity.AirportId}, fecha_ingreso={entity.HireDate:yyyy-MM-dd}, activo={entity.IsActive}.",
                ex);
        }
    }

    private static StaffEntity MapToEntity(StaffMember staff)
    {
        return new StaffEntity
        {
            Id = staff.Id.Value,
            PersonId = staff.PersonId.Value,
            RoleId = staff.RoleId.Value,
            AirlineId = staff.AirlineId.Value,
            AirportId = staff.AirportId.Value,
            HireDate = staff.HireDate.Value,
            IsActive = staff.IsActive.Value
        };
    }

    private static string NormalizeSearch(string value)
    {
        return (value ?? string.Empty).Trim().ToUpperInvariant();
    }
}

