using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Entity.Models
{
    public class Nota
    {
        [Key]
        public int NotaId { get; set; }

        [Required]
        public int EstudianteId { get; set; }

        [ForeignKey(nameof(EstudianteId))]
        public Estudiante Estudiante { get; set; } = null!;

        [Required, MaxLength(150)]
        public string Materia { get; set; } = null!;

        [Required, MaxLength(10)]
        public string Ciclo { get; set; } = "02-2025";

        // La precisión (5,2) la configuraremos en el DbContext (Bloque 3)
        public decimal Valor { get; set; }
    }
}
