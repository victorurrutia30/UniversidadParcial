using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Models
{
    public class Carrera
    {
        [Key]
        public int CarreraId { get; set; }

        [Required, MaxLength(150)]
        public string Nombre { get; set; } = null!;

        // FK a Facultad
        [Required]
        public int FacultadId { get; set; }

        [ForeignKey(nameof(FacultadId))]
        public Facultad Facultad { get; set; } = null!;

        // Relación 1:N con Estudiante
        public ICollection<Estudiante> Estudiantes { get; set; } = new List<Estudiante>();
    }
}
