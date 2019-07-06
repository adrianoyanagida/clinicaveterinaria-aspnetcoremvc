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
        private readonly IVeterinarioRepository _veterinarioRepository;
        private readonly IAnimalRepository _animalRepository;

        public ConsultaRepository(
            AppDbContext appDbContext,
            IVeterinarioRepository veterinarioRepository,
            IAnimalRepository animalRepository
            )
        {
            _appDbContext = appDbContext;
            _veterinarioRepository = veterinarioRepository;
            _animalRepository = animalRepository;
        }

        public void Add<T>(T entity) where T : class
        {
            _appDbContext.Add(entity);
        }

        public void Remove<T>(T entity) where T : class
        {
            _appDbContext.Remove(entity);
        }

        public void Update<T>(T entity) where T : class
        {
            _appDbContext.Update(entity);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _appDbContext.SaveChangesAsync()) > 0;
        }

        public IEnumerable<Consulta> GetConsultas()
        {
            var consultas = _appDbContext.Consultas
                .Include(v => v.Veterinario)
                .Include(a => a.Animal)
                .ThenInclude(a => a.User);

            var consultasOrdered = consultas
                .OrderBy(c => c.IsConcluido)
                .ThenBy(d => d.DataConsulta)
                .ThenBy(h => h.HorarioConsulta);

            return consultasOrdered.AsNoTracking();
        }

        public async Task<Consulta> GetConsultaById(int id)
        {
            var consultas = _appDbContext.Consultas
                .Include(v => v.Veterinario)
                .Include(a => a.Animal)
                .ThenInclude(a => a.User);

            var consultaById = await consultas.AsNoTracking().FirstOrDefaultAsync(i => i.ConsultaId == id);

            return consultaById;
        }

        public IEnumerable<Consulta> GetConsultasByOwnerId(string userId)
        {
            var consultas = _appDbContext.Consultas
                .Include(v => v.Veterinario)
                .Include(a => a.Animal)
                .ThenInclude(a => a.User);

            var consultasByUser = consultas.Where(i => i.Animal.UserId == userId);

            var consultasByUserOrdered = consultasByUser
                .OrderBy(d => d.DataConsulta)
                .ThenBy(h => h.HorarioConsulta);

            return consultasByUserOrdered.AsNoTracking();
        }

        public IEnumerable<Consulta> GetConsultaByDateAndVet(string date, int vetId)
        {
            var consultas = _appDbContext.Consultas
                .Include(v => v.Veterinario)
                .Include(a => a.Animal)
                .ThenInclude(a => a.User);
            
            var consultasByDataAndVet = consultas
                .Where(d => d.DataConsulta == date)
                .Where(v => v.VeterinarioId == vetId);

            return consultasByDataAndVet.AsNoTracking();
        }

        public IEnumerable<Consulta> GetConsultaByDateAndVetAndTime(string date, int vetId, string time)
        {
            var consultas = _appDbContext.Consultas
                .Include(v => v.Veterinario)
                .Include(a => a.Animal)
                .ThenInclude(a => a.User);

            var consultasByDataAndVet = consultas
                .Where(d => d.DataConsulta == date)
                .Where(v => v.VeterinarioId == vetId)
                .Where(t => t.HorarioConsulta == time);

            return consultasByDataAndVet.AsNoTracking();
        }

        public void CreateConsulta(FormularioViewModel fVM)
        {
            DateTime dateConverted = Convert.ToDateTime(fVM.DataConsulta);
            var dataConsulta = dateConverted.ToShortDateString();


            var consulta = new Consulta()
            {
                VeterinarioId = fVM.VeterinarioId,
                AnimalId = fVM.AnimalId,
                DataConsulta = dataConsulta,
                HorarioConsulta = fVM.HorarioEscolhido,
                DescricaoDoProblema = fVM.DescricaoDoProblema,
                IsVerificado = false,
                IsConcluido = false
            };
            _appDbContext.Add(consulta);
        }
    }
}
