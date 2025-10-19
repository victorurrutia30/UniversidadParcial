using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DAL.Context
{
    public class UniversidadContext : DbContext
    {
        public UniversidadContext(DbContextOptions<UniversidadContext> options)
    : base(options) { }

        public DbSet<Facultad> Facultades => Set<Facultad>();
        public DbSet<Carrera> Carreras => Set<Carrera>();
        public DbSet<Estudiante> Estudiantes => Set<Estudiante>();
        public DbSet<Nota> Notas => Set<Nota>();
        public DbSet<Materia> Materias => Set<Materia>(); //nuevo para materia

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // FACULTAD
            modelBuilder.Entity<Facultad>(e =>
            {
                e.HasKey(x => x.FacultadId);
                e.Property(x => x.Nombre).HasMaxLength(150).IsRequired();
            });

            // CARRERA
            modelBuilder.Entity<Carrera>(e =>
            {
                e.HasKey(x => x.CarreraId);
                e.Property(x => x.Nombre).HasMaxLength(150).IsRequired();

                e.HasOne(x => x.Facultad)
                 .WithMany(f => f.Carreras)
                 .HasForeignKey(x => x.FacultadId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // ESTUDIANTE
            modelBuilder.Entity<Estudiante>(e =>
            {
                e.HasKey(x => x.EstudianteId);
                e.Property(x => x.Carne).HasMaxLength(20).IsRequired();
                e.HasIndex(x => x.Carne).IsUnique();
                e.Property(x => x.Nombres).HasMaxLength(150).IsRequired();
                e.Property(x => x.Apellidos).HasMaxLength(150).IsRequired();

                e.HasOne(x => x.Carrera)
                 .WithMany(c => c.Estudiantes)
                 .HasForeignKey(x => x.CarreraId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // NOTA
            modelBuilder.Entity<Nota>(e =>
            {
                e.HasKey(x => x.NotaId);
                e.Property(x => x.Materia).HasMaxLength(150).IsRequired();
                e.Property(x => x.Ciclo).HasMaxLength(10).IsRequired();
                e.Property(x => x.Valor).HasPrecision(5, 2);

                e.HasOne(x => x.Estudiante)
                 .WithMany(s => s.Notas)
                 .HasForeignKey(x => x.EstudianteId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            // MATERIA
            modelBuilder.Entity<Materia>(e =>
            {
                e.HasKey(x => x.MateriaId);
                e.Property(x => x.Nombre).HasMaxLength(150).IsRequired();
                e.HasIndex(x => x.Nombre).IsUnique();
                e.Property(x => x.Creditos).IsRequired();
            });

            // Seed opcional
            modelBuilder.Entity<Facultad>().HasData(new Facultad { FacultadId = 1, Nombre = "Ingeniería" });
            modelBuilder.Entity<Carrera>().HasData(new Carrera { CarreraId = 1, Nombre = "Sistemas", FacultadId = 1 });
            modelBuilder.Entity<Estudiante>().HasData(new Estudiante { EstudianteId = 1, Carne = "27-0000-2025", Nombres = "Juan", Apellidos = "Pérez", CarreraId = 1 });
            modelBuilder.Entity<Nota>().HasData(new Nota { NotaId = 1, EstudianteId = 1, Materia = "Programación 1", Ciclo = "02-2025", Valor = 9.50m });
        }

    }
}
