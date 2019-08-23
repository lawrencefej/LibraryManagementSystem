using LMSRepository.Data;
using LMSRepository.Interfaces;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LMSRepository.DataAccess
{
    //public abstract class BaseRepository<T> : IBaseRepository<T> where T : class
    //{
    //    private readonly DataContext _dataContext;

    //    public BaseRepository(DataContext dataContext)
    //    {
    //        _dataContext = dataContext;
    //    }

    //    public void Create(T entity)
    //    {
    //        _dataContext.Set<T>().Add(entity);
    //    }

    //    public void Delete(T entity)
    //    {
    //        _dataContext.Set<T>().Remove(entity);
    //    }

    //    public IQueryable<T> FindAll()
    //    {
    //        return _dataContext.Set<T>();
    //    }

    //    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
    //    {
    //        return _dataContext.Set<T>()
    //            .Where(expression);
    //    }

    //    public async Task SaveAsync()
    //    {
    //        await _dataContext.SaveChangesAsync();
    //    }

    //    public void Update(T entity)
    //    {
    //        _dataContext.Set<T>().Update(entity);
    //    }
    //}
}