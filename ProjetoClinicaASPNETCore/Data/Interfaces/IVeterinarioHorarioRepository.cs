using ProjetoClinicaASPNETCore.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoClinicaASPNETCore.Data.Interfaces
{
    public interface IVeterinarioHorarioRepository
    {
        IEnumerable<VeterinarioHorario> VeterinarioHorariosById(int vetId);
    }
}
