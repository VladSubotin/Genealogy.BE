using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IRepositories
{
    public interface IRepository<T, K> where T : class
    {
        IEnumerable<T> GetAll();
        T? GetById(K id);
        IEnumerable<T> GetByField(Expression<Func<T, bool>> predicate, int? limit = null);
        void Add(T entity);
        void Update(T entity);
        void Delete(K id);
        public IEnumerable<T> GetByQuery(Func<IQueryable<T>, IQueryable<T>> query);
    }
}
