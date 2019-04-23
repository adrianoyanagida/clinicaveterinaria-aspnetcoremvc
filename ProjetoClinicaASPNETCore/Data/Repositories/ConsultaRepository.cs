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

        public IEnumerable<Animal> GetAnimaisByOwnerId(string userId) => _appDbContext.Animais.Where(a => a.User.Id == userId);

        public IEnumerable<Veterinario> Veterinarios => _appDbContext.Veterinarios;

        public IEnumerable<Consulta> Consultas => _appDbContext.Consultas.Include(a => a.Animal).Include(v => v.Veterinario).Include(u => u.User);

        public async Task<Veterinario> GetVetById(int vetId) => await _appDbContext.Veterinarios.FirstOrDefaultAsync(p => p.VeterinarioId == vetId);

        public IEnumerable<Horario> Horarios => _appDbContext.Horarios;

        public IEnumerable<Consulta> GetConsultaByDateAndVet(string date, int vetId) =>
            _appDbContext.Consultas
            .Where(d => d.DataConsulta == date)
            .Where(v => v.VeterinarioId == vetId);

        public void CreateConsulta(FormularioViewModel fVM, string userId)
        {
            var consulta = new Consulta()
            {
                VeterinarioId = fVM.VeterinarioId,
                UserId = userId,
                AnimalId = fVM.AnimalId,
                DataConsulta = fVM.DataConsulta,
                HorarioConsulta = fVM.HorarioEscolhido,
                DescricaoDoProblema = fVM.DescricaoDoProblema,
                IsActive = false
            };
            Add(consulta);
        }
    }
}
