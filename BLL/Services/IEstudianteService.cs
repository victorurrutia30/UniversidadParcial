using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BLL.Services
{
    public interface IEstudianteService
    {
        Task<List<Estudiante>> ListarAsync();
        Task<Estudiante?> ObtenerPorIdAsync(int id);
        Task<Estudiante> CrearAsync(Estudiante nuevo);
        Task<Estudiante?> ActualizarAsync(Estudiante actualizado);
        Task<bool> EliminarAsync(int id);
    }
}
