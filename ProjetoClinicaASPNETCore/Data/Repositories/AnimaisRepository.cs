using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ProjetoClinicaASPNETCore.Data.Interfaces;
using ProjetoClinicaASPNETCore.Data.Models;

namespace ProjetoClinicaASPNETCore.Data.Repositories
{
    public class AnimaisRepository : IAnimaisRepository
    {
        private AppDbContext _appDbContext { get; }
        public AnimaisRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public IEnumerable<Animal> GetAnimaisById(string id) => 
            _appDbContext.Animais
                .Where(i => i.UserId == id)
                .Include(c => c.Consultas)
                .Include(c => c.User);
    }
}