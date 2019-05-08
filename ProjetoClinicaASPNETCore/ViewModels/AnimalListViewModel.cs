using ProjetoClinicaASPNETCore.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoClinicaASPNETCore.ViewModels
{
    public class AnimalListViewModel
    {
        public IEnumerable<Animal> Animais { get; set; }
    }
}
