using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace DataCore
{
    
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private CustomerOrderContext _dbcontext;

        public Repository(CustomerOrderContext contextDb)
        {
            _dbcontext = contextDb;

        }

        public DbSet<TEntity> Set
        {
            get
            {

                return _dbcontext.Set<TEntity>();
            }
        }

        public IQueryable<TEntity> GetAll()
        {
            return Set.AsQueryable();
        }

        public IQueryable<TEntity> Find(Func<TEntity, bool> expresion)
        {

            return Set.Where(expresion).AsQueryable();
        }

        public void Delete(TEntity entity)
        {


            var entityObject = _dbcontext.Entry(entity);
            if (entityObject.State != EntityState.Deleted)
            {
                entityObject.State = EntityState.Deleted;
            }
            else
            {
                Set.Remove(entity);
            }
        }
        public void Add(TEntity entity)
        {
            var entityObject = _dbcontext.Entry(entity);
            if (entityObject.State != EntityState.Detached)
            {
                entityObject.State = EntityState.Added;
            }
            else
            {
                Set.Add(entity);
                Save();

            }

        }

        public void Update(TEntity entity)
        {

            var entry = _dbcontext.Entry(entity);

            if (entry.State == EntityState.Detached)
            {

                Set.Attach(entity);

            }

            //_dbcontext.db
            entry.State = EntityState.Modified;


        }

        public void Save()
        {
            _dbcontext.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_dbcontext != null)
                {
                    _dbcontext.Dispose();

                }
            }
        }

    }
}


