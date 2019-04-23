using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ProjetoClinicaASPNETCore.Data.Models;

namespace ProjetoClinicaASPNETCore.ViewModels
{
    public class FormularioViewModel
    {
        [Required]
        public int AnimalId { get; set; }
        [Required]
        [StringLength(500, MinimumLength = 30)]
        public string DescricaoDoProblema { get; set; }
        [Required]
        public string HorarioEscolhido {get;set;}
        [Required]
        public int VeterinarioId { get; set; }
        [Required]
        public string DataConsulta { get; set; }
        
        public List<string> HorariosFiltrados {get; set;}
        public IEnumerable<Animal> Animais {get;set;}
        public Veterinario Veterinario { get; set; }
    }
}