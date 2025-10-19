using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Entity.Models
{
    public class Materia
    {
        [Key]
        public int MateriaId { get; set; }

        [Required, MaxLength(150)]
        public string Nombre { get; set; } = null!;

        public int Creditos { get; set; } = 4;
    }
}
