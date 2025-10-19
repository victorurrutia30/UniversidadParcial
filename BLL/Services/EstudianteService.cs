using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Context;
using Entity.Models;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services
{
    public class EstudianteService : IEstudianteService
    {
        private readonly UniversidadContext _ctx;

        public EstudianteService(UniversidadContext ctx)
        {
            _ctx = ctx;
        }

        public Task<List<Estudiante>> ListarAsync()
        {
            return _ctx.Estudiantes
                       .Include(e => e.Carrera)
                           .ThenInclude(c => c.Facultad)
                       .AsNoTracking()
                       .OrderBy(e => e.Apellidos).ThenBy(e => e.Nombres)
                       .ToListAsync();
        }

        public Task<Estudiante?> ObtenerPorIdAsync(int id)
        {
            return _ctx.Estudiantes
                       .Include(e => e.Carrera)
                           .ThenInclude(c => c.Facultad)
                       .AsNoTracking()
                       .FirstOrDefaultAsync(e => e.EstudianteId == id);
        }

        // --- NUEVO: buscar por carné exacto
        public Task<Estudiante?> BuscarPorCarneAsync(string carne)
        {
            return _ctx.Estudiantes
                       .Include(e => e.Carrera)
                           .ThenInclude(c => c.Facultad)
                       .AsNoTracking()
                       .FirstOrDefaultAsync(e => e.Carne == carne);
        }

        // --- NUEVO: búsqueda libre por texto y/o carrera
        public Task<List<Estudiante>> BuscarAsync(string? texto, int? carreraId)
        {
            var query = _ctx.Estudiantes
                            .Include(e => e.Carrera)
                                .ThenInclude(c => c.Facultad)
                            .AsNoTracking()
                            .AsQueryable();

            if (!string.IsNullOrWhiteSpace(texto))
            {
                var t = texto.Trim();
                query = query.Where(e =>
                    e.Carne.Contains(t) ||
                    e.Nombres.Contains(t) ||
                    e.Apellidos.Contains(t));
            }

            if (carreraId.HasValue)
            {
                query = query.Where(e => e.CarreraId == carreraId.Value);
            }

            return query
                .OrderBy(e => e.Apellidos).ThenBy(e => e.Nombres)
                .ToListAsync();
        }

        public async Task<Estudiante> CrearAsync(Estudiante nuevo)
        {
            _ctx.Estudiantes.Add(nuevo);
            await _ctx.SaveChangesAsync();
            return nuevo;
        }

        public async Task<Estudiante?> ActualizarAsync(Estudiante actualizado)
        {
            var existe = await _ctx.Estudiantes.FindAsync(actualizado.EstudianteId);
            if (existe is null) return null;

            // Actualiza campos permitidos
            existe.Carne = actualizado.Carne;
            existe.Nombres = actualizado.Nombres;
            existe.Apellidos = actualizado.Apellidos;
            existe.CarreraId = actualizado.CarreraId;

            await _ctx.SaveChangesAsync();
            return existe;
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var existe = await _ctx.Estudiantes.FindAsync(id);
            if (existe is null) return false;

            _ctx.Estudiantes.Remove(existe);
            await _ctx.SaveChangesAsync();
            return true;
        }
    }
}
