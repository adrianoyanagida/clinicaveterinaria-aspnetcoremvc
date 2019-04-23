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
        void Add<T>(T entity) where T : class;
        Task<bool> SaveChangesAsync();

        IEnumerable<Animal> GetAnimaisByOwnerId(string userId);
        IEnumerable<Veterinario> Veterinarios { get; }
        IEnumerable<Consulta> Consultas { get; }
        Task<Veterinario> GetVetById(int vetId);
        void CreateConsulta(FormularioViewModel fVM, string userId);
        IEnumerable<Consulta> GetConsultaByDateAndVet(string date, int vetId);
        IEnumerable<Horario> Horarios { get; }   
    }
}
