using Application.Interfaces.IRepositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class Repository<T, K> : IRepository<T, K> where T : class
    {
        private readonly GenealogyDbContext dbContext;
        private readonly DbSet<T> dbSet;

        public Repository(GenealogyDbContext dbContext)
        {
            this.dbContext = dbContext;
            dbSet = dbContext.Set<T>();
        }

        public void Add(T entity)
        {
            dbSet.Add(entity);
            dbContext.SaveChanges();
        }

        public void Delete(K id)
        {
            T entity = dbSet.Find(id);
            if (entity != null)
            {
                dbSet.Remove(entity);
                dbContext.SaveChanges();
            }
        }

        public IEnumerable<T> GetAll() => dbSet.ToList();

        public IEnumerable<T> GetByField(Expression<Func<T, bool>> predicate, int? limit = null)
        {
            var query = dbSet.Where(predicate);
            query = limit == null ? query : query.Take((int)limit);

            return query.ToList();
        }

        public IEnumerable<T> GetByQuery(Func<IQueryable<T>, IQueryable<T>> query)
        {
            var resultQuery = query(dbSet);
            return resultQuery.ToList();
        }

        public T? GetById(K id) => dbSet.Find(id);

        public void Update(T entity)
        {
            dbSet.Update(entity);
            dbContext.SaveChanges();
        }
    }
}
