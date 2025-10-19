using BLL.Services;
using DAL.Context;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Entity.Repositories;
using DAL.Repositories;


namespace IOC
{
    public static class DependencyInjection
    {
        /// <summary>
        /// Registra DbContext (SQL Server) y servicios de negocio.
        /// </summary>
        public static IServiceCollection AddUniversidadCore(
            this IServiceCollection services,
            string connectionString)
        {
            // DbContext apuntando a SQL Server
            services.AddDbContext<UniversidadContext>(opts =>
                opts.UseSqlServer(connectionString));

            // Servicios BLL
            services.AddScoped<IEstudianteService, EstudianteService>();

            // Repositorios
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IMateriasRepository, MateriasRepository>();


            return services;
        }

    }
}
