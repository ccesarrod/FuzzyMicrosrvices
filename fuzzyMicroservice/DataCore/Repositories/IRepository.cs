using System;
using System.Linq;
using System.Linq.Expressions;

namespace DataCore.Repository
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {
        IQueryable<TEntity> GetAll();
        IQueryable<TEntity> Find(Func<TEntity, bool> expresion);
        IQueryable<TEntity> SearchFor(Expression<Func<TEntity, bool>> predicate);
        void Delete(TEntity entity);
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Save();

    }
}
