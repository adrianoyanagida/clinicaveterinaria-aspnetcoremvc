using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using ProjetoClinicaASPNETCore.Data;
using ProjetoClinicaASPNETCore.Data.Interfaces;
using ProjetoClinicaASPNETCore.Data.Models;
using ProjetoClinicaASPNETCore.Data.Repositories;

namespace ProjetoClinicaASPNETCore
{
    public class Startup
    {
        private IConfigurationRoot _configurationRoot;

        public Startup(IHostingEnvironment hostingEnvironment)
        {
            _configurationRoot = new ConfigurationBuilder()
            .SetBasePath(hostingEnvironment.ContentRootPath)
            .AddJsonFile("appsettings.json")
            .Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options =>options.UseSqlServer(_configurationRoot.GetConnectionString("DefaultConnection")));

            //Adiciona um identity Custom chamado ApplicationUser
            //Options determina o que é necessário para senha
            services.AddIdentity<ApplicationUser, ApplicationRole> (options =>
            {
                options.Password.RequiredLength = 4;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
            })
                .AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IAnimalRepository, AnimalRepository>();
            services.AddTransient<IConsultaRepository, ConsultaRepository>();
            services.AddTransient<IConsultasRepository, ConsultasRepository>();
            services.AddTransient<ITempRepository, TempRepository>();

            services.AddSession();
            services.AddMvc();
            services.AddAutoMapper();
        }

        public void Configure(
                IApplicationBuilder app,
                IHostingEnvironment env,
                AppDbContext context,
                RoleManager<ApplicationRole> roleManager,
                UserManager<ApplicationUser> userManager)
        {
            app.UseDeveloperExceptionPage();
            app.UseAuthentication();
            app.UseSession();
            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                routes.MapRoute(name: "EditId", template: "Consulta/{action}/{idConsulta?}", defaults: new { Controller = "Consulta", action = "Editar" });
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            DummyData.Initialize(context, userManager, roleManager).Wait();
        }
    }
}
