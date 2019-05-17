using Microsoft.EntityFrameworkCore;
using ProjetoClinicaASPNETCore.Data.Interfaces;
using ProjetoClinicaASPNETCore.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoClinicaASPNETCore.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _appDbContext;

        public UserRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<ApplicationUser> GetUser(string userId) => await _appDbContext.Users.Where(u => u.Id == userId)
            .Include(a => a.Animais)
            .ThenInclude(c => c.Consultas)
            .FirstOrDefaultAsync();
    }
}
