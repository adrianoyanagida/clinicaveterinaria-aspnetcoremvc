using ProjetoClinicaASPNETCore.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoClinicaASPNETCore.ViewModels
{
    public class AdminConsultaVM
    {
        public IEnumerable<Veterinario> Veterinarios { get; set; }
        public Animal Animal { get; set; }
        public Veterinario Veterinario { get; set; }
        public int VeterinarioId { get; set; }
        public int AnimalId { get; set; }

        public List<string> DiasDisponiveis { get; set; }
    }
}
