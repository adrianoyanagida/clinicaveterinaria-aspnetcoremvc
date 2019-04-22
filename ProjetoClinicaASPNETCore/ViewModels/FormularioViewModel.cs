using System.Collections.Generic;
using ProjetoClinicaASPNETCore.Data.Models;

namespace ProjetoClinicaASPNETCore.ViewModels
{
    public class FormularioViewModel
    {
        public int AnimalId { get; set; }
        public string DescricaoDoProblema { get; set; }
        public string HorarioEscolhido {get;set;}
        
        public List<string> HorariosFiltrados {get; set;}
        public IEnumerable<Animal> Animais {get;set;}
        public Veterinario Veterinario { get; set; }
        public string DataConsulta { get; set; }
    }
}