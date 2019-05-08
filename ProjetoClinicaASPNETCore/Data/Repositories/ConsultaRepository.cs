using Microsoft.EntityFrameworkCore;
using ProjetoClinicaASPNETCore.Data.Interfaces;
using ProjetoClinicaASPNETCore.Data.Models;
using ProjetoClinicaASPNETCore.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoClinicaASPNETCore.Data.Repositories
{
    public class ConsultaRepository : IConsultaRepository
    {
        private readonly AppDbContext _appDbContext;

        public ConsultaRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public void Add<T>(T entity) where T : class
        {
            _appDbContext.Add(entity);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _appDbContext.SaveChangesAsync()) > 0;
        }

        public IEnumerable<Veterinario> Veterinarios => _appDbContext.Veterinarios;

        public async Task<Veterinario> GetVetById(int vetId) => await _appDbContext.Veterinarios.FirstOrDefaultAsync(p => p.VeterinarioId == vetId);

        public async Task<ApplicationUser> GetUser(string userId) => await _appDbContext.Users.Where(u => u.Id == userId)
            .Include(a => a.Animais)
            .FirstOrDefaultAsync();

        public IEnumerable<Horario> Horarios => _appDbContext.Horarios.OrderBy(h => h.Hora);

        public IEnumerable<Consulta> GetConsultasByOwnerId(string userId)
        {
            return _appDbContext.Consultas
                .Where(i => i.UserId == userId)
                .Include(a => a.Animal)
                .Include(u => u.User)
                .Include(v => v.Veterinario);
        }

        public IEnumerable<Consulta> GetConsultaByDateAndVet(string date, int vetId) =>
            _appDbContext.Consultas
            .Where(d => d.DataConsulta == date)
            .Where(v => v.VeterinarioId == vetId);

        public async Task<Animal> GetAnimalById(int animalId) => await _appDbContext.Animais.FirstOrDefaultAsync(a => a.AnimalId == animalId);

        public void CreateConsulta(FormularioViewModel fVM, ApplicationUser user)
        {
            var consulta = new Consulta()
            {
                Veterinario = GetVetById(fVM.VeterinarioId).Result,
                Animal = GetAnimalById(fVM.AnimalId).Result,
                DataConsulta = fVM.DataConsulta,
                HorarioConsulta = fVM.HorarioEscolhido,
                DescricaoDoProblema = fVM.DescricaoDoProblema,
                User = user,
                IsVerificado = false,
                IsConcluido = false
            };
            Add(consulta);
        }
    }
}
