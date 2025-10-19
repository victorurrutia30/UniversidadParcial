using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace DAL.Context
{
    public class UniversidadContextFactory : IDesignTimeDbContextFactory<UniversidadContext>
    {
        public UniversidadContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<UniversidadContext>();

            // SQL Server instalado (instancia nombrada)
            var cs = "Server=Victor\\MSSQLSERVER2022;Database=UniversidadDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True";

            optionsBuilder.UseSqlServer(cs);
            return new UniversidadContext(optionsBuilder.Options);
        }

    }
}
