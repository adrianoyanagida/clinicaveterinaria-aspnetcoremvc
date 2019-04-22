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
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public string AnimalNome { get; set; }
        public string AnimalTipo { get; set; }
        public string AnimalRaca { get; set; }
        public string AnimalDataDeNascimento { get; set; }
        public string Alergico { get; set; }
    }
}
