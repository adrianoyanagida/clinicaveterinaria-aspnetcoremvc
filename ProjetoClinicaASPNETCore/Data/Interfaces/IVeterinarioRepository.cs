using ProjetoClinicaASPNETCore.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoClinicaASPNETCore.Data.Interfaces
{
    public interface IVeterinarioRepository
    {
        IEnumerable<Veterinario> Veterinarios { get; }
        Task<Veterinario> GetVetById(int vetId);
    }
}
