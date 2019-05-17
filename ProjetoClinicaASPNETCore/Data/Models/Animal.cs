using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoClinicaASPNETCore.Data.Models
{
    public class Animal
    {
        [Key]
        public int AnimalId { get; set; }
        [Required]
        public string AnimalNome { get; set; }
        [Required]
        public string AnimalTipo { get; set; }
        public string AnimalRaca { get; set; }
        [DataType(DataType.Date)]
        public string AnimalDataDeNascimento { get; set; }
        public string Alergico { get; set; }
        public List<Consulta> Consultas { get; set; }

        //FK
        [Required]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
