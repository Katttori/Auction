using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        internal AuctionContext database;
        internal DbSet<TEntity> dbSet;

        public GenericRepository(AuctionContext context)
        {
            database = context;
            dbSet = context.Set<TEntity>();
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            IQueryable<TEntity> query = dbSet;
            return query.ToList();
        }

        public TEntity Get(int id)
        {
            return dbSet.Find(id);
        }

        public void Create(TEntity entity)
        {
            dbSet.Add(entity);
            database.SaveChanges();
        }

        public void Update(TEntity entity)
        {
            database.Entry(entity).State = EntityState.Modified;
            database.SaveChanges();
        }

        public IEnumerable<TEntity> Find(Func<TEntity, Boolean> predicate)
        {
            return dbSet.Where(predicate);
        }

        public void Delete(int id)
        {
            TEntity entity = dbSet.Find(id);
            if (entity != null)
                dbSet.Remove(entity);
            database.SaveChanges();
        }
    }
}

