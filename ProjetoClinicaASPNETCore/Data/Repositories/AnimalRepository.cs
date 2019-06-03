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

        //
        public void Update<T>(T entity) where T : class
        {
            _appDbContext.Update(entity);
        }

        public void Remove<T>(T entity) where T : class
        {
            _appDbContext.Remove(entity);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _appDbContext.SaveChangesAsync()) > 0;
        }

        public async Task<Animal> GetAnimalById(int id)
        {
            var animais = _appDbContext.Animais
                .Include(u => u.User)
                .Include(c => c.Consultas);

            var animalById = await animais.AsNoTracking().FirstOrDefaultAsync(a => a.AnimalId == id);

            return animalById;
        }

        public IEnumerable<Animal> GetAnimaisByUserId(string id)
        {
            var animais = _appDbContext.Animais
                .Include(u => u.User)
                .Include(c => c.Consultas);

            var animaisByUserId = animais.AsNoTracking().Where(i => i.UserId == id);

            return animaisByUserId;
        }

        public IEnumerable<Animal> GetAllAnimais() =>
            _appDbContext.Animais
                .Include(u => u.User)
                .Include(c => c.Consultas)
                .ThenInclude(v => v.Veterinario)
                .AsNoTracking();
    }
}
