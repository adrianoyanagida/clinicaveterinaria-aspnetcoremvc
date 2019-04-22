using ProjetoClinicaASPNETCore.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoClinicaASPNETCore.ViewModels
{
    public class ConsultaViewModel
    {
        [Required]
        public int VeterinarioId { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public string DataConsulta { get; set; }

        public IEnumerable<Veterinario> Veterinarios { get; set; }
    }
}
