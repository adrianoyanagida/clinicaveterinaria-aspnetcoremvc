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
        [Required]
        public string DataConsulta { get; set; }
        [Required]
        public string HorarioConsulta { get; set; }
        public string DescricaoDoProblema { get; set; }
        public string ValorConsulta { get; set; }
        public string Diagnostico { get; set; }
        public bool IsVerificado { get; set; }
        public bool IsConcluido { get; set;}

        [Required]
        public int AnimalId { get; set; }
        public Animal Animal { get; set; }

        [Required]
        public int VeterinarioId { get; set; }
        public Veterinario Veterinario { get; set; }

    }
}
