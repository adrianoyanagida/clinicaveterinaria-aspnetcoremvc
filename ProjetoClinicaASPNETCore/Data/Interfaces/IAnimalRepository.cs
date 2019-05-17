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
        void RegisterAnimal(Animal animal, ApplicationUser user);

        //

        void Update<T>(T entity) where T : class;
        void Remove<T>(T entity) where T : class;
        Task<bool> SaveChangesAsync();
        Task<Animal> GetAnimalById(int id);
        IEnumerable<Animal> GetAnimaisByUserId(string id);
        IEnumerable<Animal> GetAllAnimais();
    }
}
