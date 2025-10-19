using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Entity.Models
{

    public class Estudiante
    {
        [Key]
        public int EstudianteId { get; set; }

        [Required, MaxLength(20)]
        public string Carne { get; set; } = null!;

        [Required, MaxLength(150)]
        public string Nombres { get; set; } = null!;

        [Required, MaxLength(150)]
        public string Apellidos { get; set; } = null!;

        [Required]
        public int CarreraId { get; set; }

        [ForeignKey(nameof(CarreraId))]
        public Carrera Carrera { get; set; } = null!;

        public ICollection<Nota> Notas { get; set; } = new List<Nota>();
    }
}
