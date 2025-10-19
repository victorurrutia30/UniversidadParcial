using BLL.Services;
using DAL.Context;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Entity.Repositories;
using DAL.Repositories;
using BLL.Storage;
using Microsoft.Extensions.Configuration;


namespace IOC
{
    public static class DependencyInjection
    {
        /// <summary>
        /// Registra DbContext (SQL Server) y servicios de negocio.
        /// </summary>
        public static IServiceCollection AddUniversidadCore(
    this IServiceCollection services,
    string connectionString,
    IConfiguration configuration)
        {
            // DbContext
            services.AddDbContext<UniversidadContext>(opts =>
                opts.UseSqlServer(connectionString));

            // Servicios BLL
            services.AddScoped<IEstudianteService, EstudianteService>();

            // Repositorios
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IMateriasRepository, MateriasRepository>();

            // ----- Firebase Storage (desde appsettings: "Firebase" section) -----
            var fbSection = configuration.GetSection("Firebase");
            var options = new FirebaseStorageOptions
            {
                Bucket = fbSection.GetValue<string>("Bucket") ?? string.Empty,
                CredentialsFile = fbSection.GetValue<string>("CredentialsFile") ?? string.Empty,
                MakePublicOnUpload = fbSection.GetValue<bool?>("MakePublicOnUpload") ?? true
            };

            services.AddSingleton(options);
            services.AddScoped<IFileStorageService, FirebaseStorageService>();

            return services;
        }


    }
}
