using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks;
using Entity.Models;

namespace Entity.Repositories
{
    /// <summary>
    /// Repositorio especifico SOLO para registrar materias, segu el enunciado
    /// </summary>
    public interface IMateriasRepository
    {
        /// <summary>
        /// Inserta una nueva materia y guarda cambios
        /// Devuelve el número de filas afectadas
        /// </summary>
        Task<int> RegistrarAsync(Materia materia);
    }
}
