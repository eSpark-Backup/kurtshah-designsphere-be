using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using designsphere_identity.Admin.EntityFramework.Shared.Entities.Application;
using designsphere_identity.STS.Identity.CustomGrants.Persistance;
using designsphere_identity.STS.Identity.CustomGrants.Repository.Contracts;
using IdentityModel;

namespace designsphere_identity.STS.Identity.CustomGrants.Repository;

public class RepositoryBase<T> : IAsyncRepository<T> where T : EntityBase
{
    protected readonly MylocalContext dbContext;

    public RepositoryBase(MylocalContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<IReadOnlyList<T>> GetAllAsync()
    {
        return await dbContext.Set<T>().AsNoTracking().ToListAsync();
    }

    public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate)
    {
        return await dbContext.Set<T>().Where(predicate).ToListAsync();
    }

    public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeString = null, bool disableTracking = true)
    {
        IQueryable<T> query = dbContext.Set<T>();
        if (disableTracking) query = query.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(includeString)) query = query.Include(includeString);

        if (predicate != null) query = query.Where(predicate);

        if (orderBy != null)
            return await orderBy(query).ToListAsync();
        return await query.ToListAsync();
    }

    public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, List<Expression<Func<T, object>>> includes = null, bool disableTracking = true)
    {
        IQueryable<T> query = dbContext.Set<T>();
        if (disableTracking) query = query.AsNoTracking();

        if (includes != null) query = includes.Aggregate(query, (current, include) => current.Include(include));

        if (predicate != null) query = query.Where(predicate);

        if (orderBy != null)
            return await orderBy(query).ToListAsync();
        return await query.ToListAsync();
    }

    public virtual async Task<T> GetByIdAsync(Guid id)
    {
        return await dbContext.Set<T>().FindAsync(id);
    }
    public virtual async Task<T> GetByIdAsync(Guid id, Expression<Func<T, bool>> predicate = null)
    {
        return await dbContext.Set<T>().Where(predicate)
            .FirstOrDefaultAsync(x=> x.Id == id);
    }

    public async Task<T> AddAsync(T entity)
    {
        dbContext.Set<T>().Add(entity);
        await dbContext.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> UpdateAsync(T entity)
    {
        dbContext.Entry(entity).State = EntityState.Modified;
        return await dbContext.SaveChangesAsync() > 0;
    }

    public async Task DeleteAsync(T entity)
    {
        dbContext.Set<T>().Remove(entity);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteByIdAsync(Guid id)
    {
        var data = dbContext.Set<T>().FindAsync(id);
        dbContext.Set<T>().Remove(data.Result);
        await dbContext.SaveChangesAsync();
    }
    public async Task<bool> SoftDeleteByIdAsync(Guid id)
    {
        var data = await dbContext.Set<T>().FindAsync(id);
        data.IsDeleted = true;
        data.IsActive = false;
        dbContext.Entry(data).State = EntityState.Modified;
        return await dbContext.SaveChangesAsync() > 0;
    }
}
