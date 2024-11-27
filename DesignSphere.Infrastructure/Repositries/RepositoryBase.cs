using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using DesignSphere.Domain.Common;
using DesignSphere.Infrastructure.Persistence;
using DesignSphere.Application.Contracts;
using BuildingBlocks.Common.Models;
using SieveCustom.Services;
using SieveCustom.Models;
using SieveCustom.Extensions;

namespace DesignSphere.Infrastructure.Repositories;

public class RepositoryBase<T> : IAsyncRepository<T> where T : EntityBase
{
    protected readonly SQLContext dbContext;
    private readonly ISieveProcessor sieveProcessor;

    public RepositoryBase(SQLContext dbContext, ISieveProcessor sieveProcessor)
    {
        this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        this.sieveProcessor = sieveProcessor;
    }

    public async Task<IReadOnlyList<T>> GetAllAsync()
    {
        return await dbContext.Set<T>().AsNoTracking().ToListAsync();
    }

    public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate)
    {
        return await dbContext.Set<T>().Where(predicate).ToListAsync();
    }
    public async Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
    {
        return await dbContext.Set<T>().Where(predicate).FirstOrDefaultAsync();
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

    public async Task<T> AddAsync(T entity)
    {
        dbContext.Set<T>().Add(entity);
        await dbContext.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(T entity)
    {
        dbContext.Entry(entity).State = EntityState.Modified;
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        dbContext.Set<T>().Remove(entity);
        await dbContext.SaveChangesAsync();
    }

    public async Task<PagedResponseOffset<T>> GetFSPOffset(SieveModel model)
    {
        var expression = sieveProcessor.GetFilteringExpression<T>(model);

        var totalRecords = await dbContext.Set<T>()
            .Where(expression)
            .AsNoTracking()
            .CountAsync();

        int page = (int)model.Page;
        int pageSize = (int)model.PageSize;
        var data = dbContext.Set<T>()
            .AsNoTracking()
            .Where(expression)
            .OrderBy(model)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return new PagedResponseOffset<T>(data, page, pageSize, totalRecords);

    }


    public async Task<PagedResponseOffset<TType>> GetFSPOffset<TType>(SieveModel model, Expression<Func<T, TType>> select) where TType : class
    {
        var expression = sieveProcessor.GetFilteringExpression<T>(model);

        var totalRecords = await dbContext.Set<T>()
            .AsNoTracking()
            .Where(expression)
            .CountAsync();
        int page = (int)model.Page;
        int pageSize = (int)model.PageSize;
        if (totalRecords == 0)
            return new PagedResponseOffset<TType>(null, page, pageSize, totalRecords);
        var data = dbContext.Set<T>()
            .AsNoTracking()
            .Where(expression)
            .Select(select)
            .OrderBy(model)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return new PagedResponseOffset<TType>(data, page, pageSize, totalRecords);
    }
}
