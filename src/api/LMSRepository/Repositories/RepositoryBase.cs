using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LMSContracts.Repository;
using LMSRepository.Data;
using Microsoft.EntityFrameworkCore;

namespace LMSRepository.Repositories
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected DataContext context;

        public RepositoryBase(DataContext context)
        {
            this.context = context;
        }

        public IQueryable<T> FindAll()
        {
            return context.Set<T>().AsNoTracking();
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return context.Set<T>().Where(expression).AsNoTracking();
        }
        // public void Create(T entity)
        // {
        //     context.Set<T>().Add(entity);
        // }

        // public T Update(T entity)
        // {
        //     return context.Set<T>().Update(entity).Entity;
        // }

        public void Delete(T entity)
        {
            context.Set<T>().Remove(entity);
        }


        public virtual async Task<T> GetByID(Guid id)
        {
            return await context.Set<T>().FindAsync(id);
        }

        public virtual async Task<T> GetByID(int id)
        {
            return await context.Set<T>().FindAsync(id);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await context.Set<T>().AsQueryable().ToListAsync();
        }
        public virtual T Create(T entity)
        {
            return context.Add(entity).Entity;
        }

        public virtual T Update(T entity)
        {
            return context.Update(entity).Entity;
        }

        public async Task SaveChanges()
        {
            await context.SaveChangesAsync();
        }
    }
}
