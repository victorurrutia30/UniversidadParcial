using IOC;
using System;

namespace WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // ----- Registro de IOC con ConnectionString + IConfiguration -----
            var cs = builder.Configuration.GetConnectionString("UniversidadDB")
                     ?? "Server=Victor\\MSSQLSERVER2022;Database=UniversidadDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True";

            IOC.DependencyInjection.AddUniversidadCore(builder.Services, cs, builder.Configuration);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();

        }
    }
}
