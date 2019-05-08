using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoClinicaASPNETCore.Data.Models
{
    public class Consulta
    {
        [Key]
        public int ConsultaId { get; set; }
        public string DataConsulta { get; set; }
        public string HorarioConsulta { get; set; }
        public string DescricaoDoProblema { get; set; }
        public string ValorConsulta { get; set; }
        public string Diagnostico { get; set; }
        public bool IsVerificado { get; set; }
        public bool IsConcluido { get; set;}

        public int AnimalId { get; set; }
        public Animal Animal { get; set; }
        
        public int VeterinarioId { get; set; }
        public Veterinario Veterinario { get; set; }
        
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
