using ProjetoClinicaASPNETCore.Data.Models;
using ProjetoClinicaASPNETCore.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoClinicaASPNETCore.Data.Interfaces
{
    public interface IConsultaRepository
    {
        IEnumerable<Animal> GetAnimaisByOwnerId(string userId);
        IEnumerable<Veterinario> Veterinarios { get; }
        IEnumerable<Consulta> Consultas { get; }
        Veterinario GetVetById(int vetId);
        //void CreateConsulta(ConsultaViewModel consultaViewModel, string userId);
        IEnumerable<Consulta> GetConsultaByDateAndVet(string date, int vetId);
        IEnumerable<Horario> Horarios { get; }
    }
}
