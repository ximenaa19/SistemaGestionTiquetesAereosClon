// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Customers\Infrastructure\Repository\CustomerRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Customers.Domain.Aggregate;
using GestionAerolineas.src.Modules.Customers.Domain.Repositories;
using GestionAerolineas.src.Modules.Customers.Domain.ValueObject;
using GestionAerolineas.src.Modules.Customers.Infrastructure.Entity;
using GestionAerolineas.src.shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestionAerolineas.src.Modules.Customers.Infrastructure.Repository;

public class CustomerRepository : ICustomerRepository
{
    private readonly AppDbContext _context;

    public CustomerRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Customer>> GetAllAsync()
    {
        var entities = await _context.Customers.AsNoTracking().ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<Customer?> GetByIdAsync(CustomerId id)
    {
        var entity = await _context.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<Customer?> GetByPersonIdAsync(CustomerPersonId personId)
    {
        var entity = await _context.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.PersonId == personId.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<Customer?> GetByPersonNameAsync(CustomerPersonName personName)
    {
        var normalizedName = CustomerPersonName.Normalize(personName.Value);

        var entity = await (
            from customer in _context.Customers.AsNoTracking()
            join person in _context.People.AsNoTracking() on customer.PersonId equals person.Id
            where ((person.FirstNames ?? string.Empty).Trim() + " " + (person.LastNames ?? string.Empty).Trim()).Trim().ToUpper() == normalizedName
            select customer
        ).FirstOrDefaultAsync();

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task AddAsync(Customer customer)
    {
        await _context.Customers.AddAsync(MapToEntity(customer));
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Customer customer)
    {
        var existing = await _context.Customers
            .FirstOrDefaultAsync(e => e.Id == customer.Id.Value);

        if (existing is null)
            return;

        existing.PersonId = customer.PersonId.Value;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Customer customer)
    {
        var entity = await _context.Customers.FindAsync(customer.Id.Value);

        if (entity is null)
            return;

        _context.Customers.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public Task<bool> ExistsAsync(CustomerId id)
    {
        return _context.Customers.AnyAsync(e => e.Id == id.Value);
    }

    public Task<bool> ExistsByPersonIdAsync(CustomerPersonId personId, CustomerId? excludingId = null)
    {
        var query = _context.Customers
            .AsNoTracking()
            .Where(c => c.PersonId == personId.Value);

        if (excludingId != null)
            query = query.Where(c => c.Id != excludingId.Value);

        return query.AnyAsync();
    }

    private static Customer MapToDomain(CustomerEntity entity)
    {
        try
        {
            return Customer.Create(
                CustomerId.Create(entity.Id),
                CustomerPersonId.Create(entity.PersonId),
                CustomerCreatedAt.Create(entity.CreatedAt)
            );
        }
        catch (Exception ex)
        {
            throw new Exception(
                $"El registro clients(id={entity.Id}) tiene datos invalidos. " +
                $"persona_id={entity.PersonId}, creado_en='{entity.CreatedAt}'.",
                ex);
        }
    }

    private static CustomerEntity MapToEntity(Customer customer)
    {
        return new CustomerEntity
        {
            Id = customer.Id.Value,
            PersonId = customer.PersonId.Value,
            CreatedAt = customer.CreatedAt.Value
        };
    }
}
