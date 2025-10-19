using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Entity.Repositories
{
    /// <summary>
    /// Repositorio genérico con soporte de filtros por predicado,
    /// consultas IQueryable y operaciones que confirman con bool.
    /// </summary>
    public interface IGenericRepository<T> where T : class
    {
        // --- CONSULTAS (Entidad o IQueryable) ---
        Task<T?> GetByIdAsync(int id);

        /// <summary>
        /// Devuelve un IQueryable para construir consultas (filtros/orden/includes) en capa superior.
        /// Por defecto usa AsNoTracking para lectura.
        /// </summary>
        IQueryable<T> Query(
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            bool asNoTracking = true,
            params Expression<Func<T, object>>[] includes);

        /// <summary>
        /// Devuelve una sola entidad según el predicado e includes opcionales.
        /// </summary>
        Task<T?> FirstOrDefaultAsync(
            Expression<Func<T, bool>> predicate,
            bool asNoTracking = true,
            params Expression<Func<T, object>>[] includes);

        // --- COMANDOS (confirman con bool) ---
        Task<bool> AddAsync(T entity);          // Inserta y confirma
        Task<bool> UpdateAsync(T entity);       // Actualiza y confirma
        Task<bool> DeleteAsync(int id);         // Elimina por Id y confirma
        Task<bool> DeleteAsync(T entity);       // Elimina una instancia y confirma

        // --- OPCIONAL: para escenarios de unidad de trabajo por lotes ---
        Task<int> SaveChangesAsync();
    }
}
