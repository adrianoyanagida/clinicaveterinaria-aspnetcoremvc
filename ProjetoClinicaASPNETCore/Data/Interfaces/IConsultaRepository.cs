using Microsoft.AspNetCore.Identity;
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

        IEnumerable<Veterinario> Veterinarios { get; }
        Task<Veterinario> GetVetById(int vetId);
        Task<ApplicationUser> GetUser(string userId);
        IEnumerable<Horario> Horarios { get; }
        IEnumerable<Consulta> GetConsultaByDateAndVet(string date, int vetId);
        void CreateConsulta(FormularioViewModel fVM, ApplicationUser user);
    }
}
