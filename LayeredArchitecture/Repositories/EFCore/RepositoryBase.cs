using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;

namespace Repositories.EFCore;

public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
    protected readonly RepositoryContext Context;

    public RepositoryBase(RepositoryContext context)
    {
        Context = context;
    }
    public IQueryable<T> FindAll(bool trackChanges) =>
        trackChanges ? Context.Set<T>() : Context.Set<T>().AsNoTracking();
    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges) => !trackChanges
        ? Context.Set<T>().Where(expression).AsNoTracking()
        : Context.Set<T>().Where(expression);
    public void Create(T entity) => Context.Set<T>().Add(entity);

    public void Update(T entity) => Context.Set<T>().Update(entity);

    public void Delete(T entity) => Context.Set<T>().Remove(entity);
}