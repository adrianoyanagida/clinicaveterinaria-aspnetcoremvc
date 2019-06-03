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

        public void Update<T>(T entity) where T : class
        {
            _appDbContext.Update(entity);
        }
        public void Remove<T>(T entity) where T : class
        {
            _appDbContext.Remove(entity);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _appDbContext.SaveChangesAsync()) > 0;
        }

        public IEnumerable<ApplicationUser> GetUsers()
        {
            var users = _appDbContext.Users
                .Include(ur => ur.UserRoles)
                .ThenInclude(r => r.Role)
                .Include(a => a.Animais)
                .ThenInclude(c => c.Consultas);

            var usersOrder = users.OrderBy(n => n.NomeCompleto);

            return usersOrder.AsNoTracking();
        }

        public async Task<ApplicationUser> GetUser(string userId)
        {
            var users = _appDbContext.Users
                .Include(a => a.Animais)
                .ThenInclude(c => c.Consultas);

            var usuarioById = users.Where(u => u.Id == userId);

            return await usuarioById.AsNoTracking().FirstOrDefaultAsync();
        }
    }
}
