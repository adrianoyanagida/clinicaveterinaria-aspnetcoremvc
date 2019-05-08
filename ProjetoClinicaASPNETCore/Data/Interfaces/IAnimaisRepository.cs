using System.Collections.Generic;
using ProjetoClinicaASPNETCore.Data.Models;

namespace ProjetoClinicaASPNETCore.Data.Interfaces
{
    public interface IAnimaisRepository
    {
        IEnumerable<Animal> GetAnimaisById(string id);
    }
}