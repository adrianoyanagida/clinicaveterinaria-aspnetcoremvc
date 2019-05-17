using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoClinicaASPNETCore.Data.Models
{
    public class Veterinario
    {
        [Key]
        public int VeterinarioId { get; set; }
        public string VetNome { get; set; }
        public string VetEndereco { get; set; }
        public string VetTelefone { get; set; }
        public string VetFuncao { get; set; }

        public List<Consulta> Consultas { get; set; }
        public List<VeterinarioHorario> VeterinarioHorarios { get; set; }
    }
}
