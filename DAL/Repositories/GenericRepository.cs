using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Entity.Repositories;
using DAL.Context;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    /// <summary>
    /// Implementación genérica sobre EF Core para cualquier entidad.
    /// Incluye filtros por predicado, proyección IQueryable y operaciones que confirman con bool.
    /// </summary>
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly UniversidadContext _ctx;
        private readonly DbSet<T> _set;

        public GenericRepository(UniversidadContext ctx)
        {
            _ctx = ctx;
            _set = _ctx.Set<T>();
        }

        // --------------------
        // CONSULTAS
        // --------------------
        public async Task<T?> GetByIdAsync(int id)
        {
            // FindAsync usa la PK configurada; para múltiples claves usarías key values[]
            return await _set.FindAsync(id);
        }

        public IQueryable<T> Query(
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            bool asNoTracking = true,
            params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _set;

            if (asNoTracking)
                query = query.AsNoTracking();

            if (includes is { Length: > 0 })
            {
                foreach (var include in includes)
                    query = query.Include(include);
            }

            if (filter != null)
                query = query.Where(filter);

            if (orderBy != null)
                query = orderBy(query);

            return query;
        }

        public async Task<T?> FirstOrDefaultAsync(
            Expression<Func<T, bool>> predicate,
            bool asNoTracking = true,
            params Expression<Func<T, object>>[] includes)
        {
            var query = Query(predicate, null, asNoTracking, includes);
            return await query.FirstOrDefaultAsync();
        }

        // --------------------
        // COMANDOS
        // --------------------
        public async Task<bool> AddAsync(T entity)
        {
            await _set.AddAsync(entity);
            return await _ctx.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            // Si viene detach, lo adjuntamos y marcamos como modificado
            var entry = _ctx.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                _set.Attach(entity);
                entry = _ctx.Entry(entity);
            }

            entry.State = EntityState.Modified;
            return await _ctx.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity is null) return false;

            _set.Remove(entity);
            return await _ctx.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(T entity)
        {
            if (_ctx.Entry(entity).State == EntityState.Detached)
            {
                _set.Attach(entity);
            }

            _set.Remove(entity);
            return await _ctx.SaveChangesAsync() > 0;
        }

        // --------------------
        // OPCIONAL (lotes)
        // --------------------
        public Task<int> SaveChangesAsync() => _ctx.SaveChangesAsync();
    }
}
