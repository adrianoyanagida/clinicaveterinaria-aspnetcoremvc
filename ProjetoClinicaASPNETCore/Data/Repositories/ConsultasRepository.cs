using Microsoft.EntityFrameworkCore;
using ProjetoClinicaASPNETCore.Data.Interfaces;
using ProjetoClinicaASPNETCore.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoClinicaASPNETCore.Data.Repositories
{
    public class ConsultasRepository : IConsultasRepository
    {
        private readonly AppDbContext _appDbContext;

        public ConsultasRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public void Remove<T>(T entity) where T : class
        {
            _appDbContext.Remove(entity);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _appDbContext.SaveChangesAsync()) > 0;
        }

        public async Task<Consulta> GetConsultaById(int id) => await _appDbContext.Consultas.FirstOrDefaultAsync(i => i.ConsultaId == id);

        public IEnumerable<Consulta> GetConsultasByOwnerId(string userId)
        {
            return _appDbContext.Consultas.Where(i => i.UserId == userId)
                .Include(a => a.Animal)
                .Include(u => u.User)
                .Include(v => v.Veterinario)
                .OrderBy(d => d.DataConsulta)
                .ThenBy(h => h.HorarioConsulta);
        }
    }
}
