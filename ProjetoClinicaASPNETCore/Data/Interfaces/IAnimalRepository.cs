using ProjetoClinicaASPNETCore.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoClinicaASPNETCore.Data.Interfaces
{
    public interface IAnimalRepository
    {
        IEnumerable<Animal> Animais { get; }
        IEnumerable<Animal> AllAnimais { get; }
        void RegisterAnimal(Animal animal, string Id);
    }
}
