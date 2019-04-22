using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProjetoClinicaASPNETCore.Data;
using ProjetoClinicaASPNETCore.Data.Interfaces;
using ProjetoClinicaASPNETCore.Data.Models;
using ProjetoClinicaASPNETCore.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoClinicaASPNETCore.Controllers
{
    [Authorize]
    public class ConsultaController : Controller
    {
        private readonly IConsultaRepository _consultaRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public ConsultaController(
            IConsultaRepository consultaRepository,
            UserManager<ApplicationUser> userManager
        )
        {
            _consultaRepository = consultaRepository;
            _userManager = userManager;
        }

        public IActionResult Data()
        {
            var animais = _consultaRepository
                .GetAnimaisByOwnerId(_userManager.GetUserId(User))
                .LongCount();

            if(animais == 0)
            {
                TempData["CadastroAnimal"] = "Você não tem um animalzinho cadastrado, cadastre um primeiro.";
                return RedirectToAction("Erro");
            }

            var veterinarios = _consultaRepository.Veterinarios.OrderBy(n => n.VetNome);

            var consultaViewModel = new ConsultaViewModel { Veterinarios = veterinarios };

            return View(consultaViewModel);
        }

        [HttpPost]
        public IActionResult Data(ConsultaViewModel cVM)
        {
            var consultas = _consultaRepository
                .GetConsultaByDateAndVet(cVM.DataConsulta, cVM.VeterinarioId);
            var horarios = _consultaRepository.Horarios;

            List<string> HorariosFiltrados = new List<string>();

            foreach(Horario horario in horarios)
            {
                foreach(Consulta consulta in consultas)
                {
                    if(horario.Hora != consulta.HorarioConsulta)
                    {
                        HorariosFiltrados.Add(horario.Hora);
                    }
                }
            }
            foreach(string horario in HorariosFiltrados)
            {
                Console.WriteLine(horario);
            }
            return RedirectToAction("Erro");
        }

        public IActionResult Erro() => View();
    }
}
