using Entity.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Services
{
    public interface IEstudianteService
    {
        // Listar todo
        Task<List<Estudiante>> ListarAsync();

        // Buscar por Id (ya lo tienes)
        Task<Estudiante?> ObtenerPorIdAsync(int id);

        // NUEVO: Buscar por carné exacto
        Task<Estudiante?> BuscarPorCarneAsync(string carne);

        // NUEVO: Buscar por texto y/o carrera (texto: carné, nombres o apellidos)
        Task<List<Estudiante>> BuscarAsync(string? texto, int? carreraId);

        // Crear / Modificar / Eliminar
        Task<Estudiante> CrearAsync(Estudiante nuevo);
        Task<Estudiante?> ActualizarAsync(Estudiante actualizado);
        Task<bool> EliminarAsync(int id);
    }
}
