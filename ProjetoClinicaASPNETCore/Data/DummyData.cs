using Microsoft.AspNetCore.Identity;
using ProjetoClinicaASPNETCore.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoClinicaASPNETCore.Data
{
    public class DummyData
    {
        public static async Task Initialize(AppDbContext context, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            context.Database.EnsureCreated();
            
            Veterinario GetVet(int vetId)
            {
                return context.Veterinarios.FirstOrDefault(v => v.VeterinarioId == vetId);
            }

            Horario GetHorario(int hrId)
            {
                return context.Horarios.FirstOrDefault(h => h.HorarioId == hrId);
            }

            //Instanciando as roles
            var adm = new AllRoles.Administrador();
            var cli = new AllRoles.Cliente();
            var fun = new AllRoles.Funcionario();

            string password = "senha";

            //Criando as roles no banco de dados
            if (await roleManager.FindByNameAsync(adm.funcao) == null)
            {
                await roleManager.CreateAsync(new ApplicationRole(adm.funcao, adm.descricao, DateTime.Now));
            }
            if (await roleManager.FindByNameAsync(cli.funcao) == null)
            {
                await roleManager.CreateAsync(new ApplicationRole(cli.funcao, cli.descricao, DateTime.Now));
            }

            if (await roleManager.FindByNameAsync(fun.funcao) == null)
            {
                await roleManager.CreateAsync(new ApplicationRole(fun.funcao, fun.descricao, DateTime.Now));
            }

            //Preenchendo essas roles com administrador, funcionario e cliente
            if (await userManager.FindByNameAsync("administrador") == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "administrador",
                    Email = "admin@email.com"
                };

                var result = await userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    await userManager.AddPasswordAsync(user, password);
                    await userManager.AddToRoleAsync(user, adm.funcao);
                }
            }

            if (await userManager.FindByNameAsync("cliente") == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "cliente",
                    Email = "cliente@email.com"
                };

                var result = await userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    await userManager.AddPasswordAsync(user, password);
                    await userManager.AddToRoleAsync(user, cli.funcao);
                }
            }
            if (await userManager.FindByNameAsync("funcionario") == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "funcionario",
                    Email = "funcionario@email.com"
                };

                var result = await userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    await userManager.AddPasswordAsync(user, password);
                    await userManager.AddToRoleAsync(user, fun.funcao);
                }
            }
            
            //Adicionando alguns Veterinarios
            if (!context.Veterinarios.Any())
            {
                context.AddRange
                (
                    new Veterinario
                    {
                        VetNome = "Vitor Bruno Guimarães",
                        VetFuncao = "Cirurgião Geral"
                    },
                    new Veterinario
                    {
                        VetNome = "Adriano Japon da Costa",
                        VetFuncao = "Clínico Geral"
                    }
                );
            }

            if (!context.Horarios.Any())
            {
                context.AddRange
                (
                    new Horario
                    {
                        Hora = "07:00"
                    },
                    new Horario
                    {
                        Hora = "08:00"
                    },
                    new Horario
                    {
                        Hora = "09:00"
                    },
                    new Horario
                    {
                        Hora = "10:00"
                    },
                    new Horario
                    {
                        Hora = "11:00"
                    },
                    new Horario
                    {
                        Hora = "12:00"
                    },
                    new Horario
                    {
                        Hora = "07:30"
                    },
                    new Horario
                    {
                        Hora = "08:30"
                    },
                    new Horario
                    {
                        Hora = "09:30"
                    },
                    new Horario
                    {
                        Hora = "10:30"
                    },
                    new Horario
                    {
                        Hora = "11:30"
                    },
                    new Horario
                    {
                        Hora = "12:30"
                    }
                );
            }

            if (!context.VeterinarioHorarios.Any())
            {
                context.AddRange
                (
                    new VeterinarioHorario
                    {
                        Veterinario = GetVet(1),
                        Horario = GetHorario(1)
                    },
                    new VeterinarioHorario
                    {
                        Veterinario = GetVet(1),
                        Horario = GetHorario(2)
                    },
                    new VeterinarioHorario
                    {
                        Veterinario = GetVet(1),
                        Horario = GetHorario(3)
                    },
                    new VeterinarioHorario
                    {
                        Veterinario = GetVet(2),
                        Horario = GetHorario(4)
                    },
                    new VeterinarioHorario
                    {
                        Veterinario = GetVet(2),
                        Horario = GetHorario(5)
                    },
                    new VeterinarioHorario
                    {
                        Veterinario = GetVet(2),
                        Horario = GetHorario(6)
                    }
                );
            }
            context.SaveChanges();
        }
    }
}
