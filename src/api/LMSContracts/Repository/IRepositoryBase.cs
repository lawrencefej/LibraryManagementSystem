using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LMSContracts.Repository
{
    public interface IRepositoryBase<T>
    {
        IQueryable<T> FindAll();
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
        Task<T> GetByID(Guid id);
        Task<T> GetByID(int id);
        T Create(T entity);
        T Update(T entity);
        void Delete(T entity);
        Task SaveChanges();
    }
}
