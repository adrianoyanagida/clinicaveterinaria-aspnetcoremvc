using Microsoft.EntityFrameworkCore;
using ProjetoClinicaASPNETCore.Data.Interfaces;
using ProjetoClinicaASPNETCore.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoClinicaASPNETCore.Data.Repositories
{
    public class VeterinarioHorarioRepository : IVeterinarioHorarioRepository
    {
        private readonly AppDbContext _appDbContext;

        public VeterinarioHorarioRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<VeterinarioHorario> VeterinarioHorariosById(int vetId)
        {
            var veterinariosHorarios = _appDbContext.VeterinarioHorarios
                .Include(h => h.Horario)
                .Include(v => v.Veterinario);

            var veterinariosHorariosById = veterinariosHorarios.Where(v => v.VeterinarioId == vetId);

            return veterinariosHorariosById;
        }
    }
}
