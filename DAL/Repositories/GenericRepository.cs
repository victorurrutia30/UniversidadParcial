using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Entity.Repositories;
using DAL.Context;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    /// <summary>
    /// Implementación genérica sobre EF Core para cualquier entidad.
    /// </summary>
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly UniversidadContext _ctx;

        public GenericRepository(UniversidadContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            // FindAsync usa las PK configuradas para la entidad
            return await _ctx.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> ListAsync(Expression<Func<T, bool>>? predicate = null)
        {
            IQueryable<T> query = _ctx.Set<T>().AsNoTracking();
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            return await query.ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _ctx.Set<T>().AddAsync(entity);
        }

        public void Update(T entity)
        {
            _ctx.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            _ctx.Set<T>().Remove(entity);
        }

        public Task<int> SaveChangesAsync()
        {
            return _ctx.SaveChangesAsync();
        }
    }
}
