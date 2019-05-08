using ProjetoClinicaASPNETCore.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoClinicaASPNETCore.Data.Interfaces
{
    public interface IConsultasRepository
    {
        void Remove<T>(T entity) where T : class;
        Task<bool> SaveChangesAsync();
        Task<Consulta> GetConsultaById(int id);
        IEnumerable<Consulta> GetConsultasByOwnerId(string userId);
    }
}
