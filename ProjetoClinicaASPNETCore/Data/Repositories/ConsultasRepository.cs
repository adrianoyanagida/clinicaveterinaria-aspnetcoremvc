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

        public IEnumerable<Consulta> GetConsultasByOwnerId(string userId)
        {
            return _appDbContext.Consultas.Where(i => i.UserId == userId)
                .Include(a => a.Animal)
                .Include(u => u.User)
                .Include(v => v.Veterinario);
        }
    }
}
