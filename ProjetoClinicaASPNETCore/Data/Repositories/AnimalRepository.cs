using Microsoft.EntityFrameworkCore;
using ProjetoClinicaASPNETCore.Data.Interfaces;
using ProjetoClinicaASPNETCore.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoClinicaASPNETCore.Data.Repositories
{
    public class AnimalRepository : IAnimalRepository
    {
        private readonly AppDbContext _appDbContext;

        public AnimalRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<Animal> Animais => _appDbContext.Animais.Include(c =>
            c.User);

        public IEnumerable<Animal> AllAnimais => _appDbContext.Animais;

        public void RegisterAnimal(Animal animal, ApplicationUser user)
        {
            animal.User = user;
            _appDbContext.Animais.Add(animal);
        }
    }
}
