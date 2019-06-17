using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjetoClinicaASPNETCore.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoClinicaASPNETCore.Data
{
    //Passa qual vai ser o Identity padrão como ApplicationUser
    public class AppDbContext : IdentityDbContext<
        ApplicationUser, 
        ApplicationRole, 
        string, 
        IdentityUserClaim<string>,
        ApplicationUserRole,
        IdentityUserLogin<string>,
        IdentityRoleClaim<string>,
        IdentityUserToken<string>
        >
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Veterinario> Veterinarios { get; set; }
        public DbSet<Animal> Animais { get; set; }
        public DbSet<Consulta> Consultas { get; set; }
        public DbSet<Horario> Horarios { get; set; }
        public DbSet<VeterinarioHorario> VeterinarioHorarios { get; set; }
        public DbSet<FeriadoERecesso> FeriadoERecessos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUserRole>(userRole =>
            {
                userRole.HasKey(ur => new { ur.UserId, ur.RoleId });
                userRole.HasOne(ur => ur.Role).WithMany(r => r.UserRoles).HasForeignKey(ur => ur.RoleId).IsRequired();
                userRole.HasOne(ur => ur.User).WithMany(r => r.UserRoles).HasForeignKey(ur => ur.UserId).IsRequired();
            });

            builder.Entity<ApplicationUser>()
                .HasIndex(u => u.Email)
                .IsUnique();

            builder.Entity<ApplicationUser>()
                .HasIndex(u => u.UserName)
                .IsUnique();

            builder.Entity<ApplicationUser>()
                .HasIndex(u => u.CPF)
                .IsUnique();

            builder.Entity<Consulta>()
                .HasIndex(u => new { u.VeterinarioId, u.DataConsulta, u.HorarioConsulta })
                .IsUnique();

            builder.Entity<VeterinarioHorario>().HasKey(VE => new { VE.VeterinarioId, VE.HorarioId });
        }
    }
}
