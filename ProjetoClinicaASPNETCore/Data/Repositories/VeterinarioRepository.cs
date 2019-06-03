using Microsoft.EntityFrameworkCore;
using ProjetoClinicaASPNETCore.Data.Interfaces;
using ProjetoClinicaASPNETCore.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoClinicaASPNETCore.Data.Repositories
{
    public class VeterinarioRepository : IVeterinarioRepository
    {
        private readonly AppDbContext _appDbContext;

        public VeterinarioRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<Veterinario> Veterinarios => _appDbContext.Veterinarios.Include(c => c.Consultas);

        public async Task<Veterinario> GetVetById(int vetId)
        {
            var veterinarios = _appDbContext.Veterinarios
                .Include(c => c.Consultas)
                .Include(v => v.VeterinarioHorarios)
                .ThenInclude(h => h.Horario);

            var veterinarioById = await veterinarios.AsNoTracking().FirstOrDefaultAsync(p => p.VeterinarioId == vetId);

            return veterinarioById;
        }
    }
}
