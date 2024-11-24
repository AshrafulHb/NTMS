using Microsoft.EntityFrameworkCore;
using NTMS.DAL.DBContext;
using NTMS.DAL.Repository.Abstract;
using System.Linq.Expressions;

namespace NTMS.DAL.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        public readonly NtmsContext Context;

        public GenericRepository(NtmsContext context)
        {
            Context = context;
        }
        public virtual async Task<T> Get(Expression<Func<T, bool>> filter)
        {
            try
            {
                T model = await Context.Set<T>().FirstOrDefaultAsync(filter);
                return model;
            }
            catch { throw; }
        }
        public virtual async Task<T> Create(T model)
        {
            try
            {
                Context.Set<T>().Add(model);
                await Context.SaveChangesAsync();
                return model;
            }
            catch { throw; }
        }



        public virtual async Task<bool> Edit(T model)
        {
            try
            {
                Context.Set<T>().Update(model);
                await Context.SaveChangesAsync();
                return true;
            }
            catch { throw; }
        }


        public virtual async Task<bool> Delete(T model)
        {
            try
            {
                Context.Set<T>().Remove(model);
                await Context.SaveChangesAsync();
                return true;
            }
            catch { throw; }
        }
        public virtual async Task<IQueryable<T>> GetAll(Expression<Func<T, bool>> filter = null)
        {
            try
            {
                IQueryable<T> query = filter == null ? Context.Set<T>() : Context.Set<T>().Where(filter);
                return query;
            }
            catch { throw; }
        }
    }
}
