using System;
using System.Linq;

namespace DataCore.Repository
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {
        IQueryable<TEntity> GetAll();
        IQueryable<TEntity> Find(Func<TEntity, bool> expresion);
        void Delete(TEntity entity);
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Save();

    }
}
