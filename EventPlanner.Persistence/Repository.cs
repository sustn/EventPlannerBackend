using EventPlanner.Application.Interfaces;
using EventPlanner.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace EventPlanner.Persistence;
public class Repository<T> : IRepository<T> where T : class, ISoftDelete
{

    private readonly ApplicationDBContext _context;
    private DbSet<T>? _entities;
    protected virtual DbSet<T> Entities => _entities ??= _context.Set<T>();
    public IQueryable<T> TableNoTracking => Entities.AsNoTracking().Where(e => e.isDeleted == false);

    public Repository(ApplicationDBContext context)
    {
        _context = context;
    }

    public async Task<bool> Add(T entity)
    {
        await Entities.AddAsync(entity);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> Add(IList<T> entities)
    {
        await Entities.AddRangeAsync(entities);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> Update(T entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> Update(IList<T> entities)
    {
        _context.UpdateRange(entities);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> Delete(T entity)
    {
        //Entities.Remove(entity);
        _context.Entry(entity).State = EntityState.Deleted;
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> Delete(IList<T> entities)
    {
        foreach (var entity in entities)
            _context.Entry(entity).State = EntityState.Deleted;
        Entities.RemoveRange(entities);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<T> AddAndGetId(T entity)
    {
        await Entities.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }
}
