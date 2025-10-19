using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
