using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Context;
using Entity.Models;
using Entity.Repositories;

namespace DAL.Repositories
{
    /// <summary>
    /// aca implementamos el repositorio especifico  para registrar materias
    /// </summary>
    public class MateriasRepository : IMateriasRepository
    {
        private readonly UniversidadContext _ctx;

        public MateriasRepository(UniversidadContext ctx)
        {
            _ctx = ctx;
        }

        /// <summary>
        /// Inserta una materia y guarda cambios
        /// </summary>
        public async Task<int> RegistrarAsync(Materia materia)
        {
            await _ctx.Materias.AddAsync(materia);
            return await _ctx.SaveChangesAsync();
        }
    }
}
