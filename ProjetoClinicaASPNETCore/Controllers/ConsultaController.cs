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

            if (animais == 0)
            {
                TempData["CadastroAnimal"] = "Você não tem um animalzinho cadastrado, cadastre um primeiro.";
                return RedirectToAction("Erro");
            }

            var veterinarios = _consultaRepository.Veterinarios.OrderBy(n => n.VetNome);

            var consultaViewModel = new ConsultaViewModel { Veterinarios = veterinarios };

            return View(consultaViewModel);
        }

        public IActionResult Formulario(ConsultaViewModel cVM)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Data");
            }

            var consultas = _consultaRepository.GetConsultaByDateAndVet(cVM.DataConsulta, cVM.VeterinarioId);
            var horarios = _consultaRepository.Horarios;
            var animais = _consultaRepository.GetAnimaisByOwnerId(_userManager.GetUserId(User));

            var veterinario = GetVeterinario(cVM.VeterinarioId).Result;
            var horariosFiltrados = GetHorariosFiltrados(consultas, horarios);

            var formularioViewModel = new FormularioViewModel
            {
                HorariosFiltrados = horariosFiltrados,
                Animais = animais,
                Veterinario = veterinario,
                DataConsulta = cVM.DataConsulta
            };

            return View(formularioViewModel);
        }

        public IActionResult Erro() => View();

        public List<string> GetHorariosFiltrados(
            IEnumerable<Consulta> consultas,
            IEnumerable<Horario> horarios)
        {
            List<string> HorariosFiltrados = new List<string>();

            foreach (Horario horario in horarios)
            {
                if(consultas.LongCount() <= 0)
                {
                    HorariosFiltrados.Add(horario.Hora);
                }
                else
                {
                    foreach (Consulta consulta in consultas)
                    {
                        if (horario.Hora != consulta.HorarioConsulta)
                        {
                            HorariosFiltrados.Add(horario.Hora);
                        }
                    }
                }
            }

            return HorariosFiltrados;
        }

        public async Task<Veterinario> GetVeterinario(int vetId)
        {
            return await _consultaRepository.GetVetById(vetId);
        }
    }
}
