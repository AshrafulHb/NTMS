using Microsoft.EntityFrameworkCore;
using NTMS.DAL.Repository.Abstract;
using NTMS.Model;
using System.Linq.Expressions;

namespace NTMS.DAL.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        //private readonly NtmsContext _context;
        public readonly NtmsContext Context;

        public GenericRepository(NtmsContext context)
        {
            _context = context;
        }
        public async Task<T> Get(Expression<Func<T, bool>> filter)
        {
            try
            {
                T model= await _context.Set<T>().FirstOrDefaultAsync(filter);
                return model;
            }
            catch { throw; }
        }
        public async Task<T> Create(T model)
        {
            try
            {
                _context.Set<T>().Add(model);
                await _context.SaveChangesAsync();  
                return model;
            }
            catch { throw; }
        }



        public async Task<bool> Edit(T model)
        {
            try
            {
                Context.Set<T>().Update(model);
                await Context.SaveChangesAsync();
                return true;
            }
            catch { throw; }
        }


        public async Task<bool> Delete(T model)
        {
            try
            {
                _context.Set<T>().Remove(model);
                await _context.SaveChangesAsync();
                return true;
            }
            catch { throw; }
        }
        public async Task<IQueryable<T>> GetAll(Expression<Func<T, bool>> filter = null)
        {
            try
            {
                IQueryable<T> query= filter==null?  _context.Set<T>():_context.Set<T>().Where(filter);
                return query;
            }
            catch { throw; }
        }
    }
}
