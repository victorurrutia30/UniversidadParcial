using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Entity.Models
{
    public class Facultad
    {
        [Key]
        public int FacultadId { get; set; }

        [Required, MaxLength(150)]
        public string Nombre { get; set; } = null!;

        // Relación 1:N con Carrera
        public ICollection<Carrera> Carreras { get; set; } = new List<Carrera>();
    }
}
