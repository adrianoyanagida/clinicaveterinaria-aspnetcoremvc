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
            var animais = GetUser().Result.Animais;

            if (animais.LongCount() == 0)
            {
                TempData["error"] = "Você não tem um animalzinho cadastrado, cadastre um primeiro.";
                return RedirectToAction(actionName: "Cadastro", controllerName: "Animal");
            }
            
            if(TemConsultasPendentes() == true)
            {
                TempData["error"] = "Você tem consultas pendentes, consulte primeiro antes de criar uma nova consulta!";
                return RedirectToAction(actionName: "SuasConsultas", controllerName: "Consultas");
            }

            var veterinarios = _consultaRepository.Veterinarios.OrderBy(n => n.VetNome);

            var consultaViewModel = new ConsultaViewModel { Veterinarios = veterinarios };

            return View(consultaViewModel);
        }

        [HttpPost]
        public IActionResult Data(ConsultaViewModel cVM)
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = "Por favor selecionar a data da consulta";
                return RedirectToAction("Data");
            }

            if(DateOnPast(cVM.DataConsulta))
            {
                TempData["error"] = "Não pode marcar uma consulta no passado";
                return RedirectToAction("Data");
            }

            CreateChoosenTempData(cVM.VeterinarioId, cVM.DataConsulta);

            return RedirectToAction("Formulario");
        }

        public IActionResult Formulario()
        {
            if (TempData["veterinarioIdEscolhido"] == null || TempData["dataEscolhida"] == null)
            {
                return RedirectToAction("Data");
            }

            var dataEscolhida = GetDataChoosenFromTemp();
            var veterinarioEscolhido = GetVetIdChoosenFromTemp();

            var fVM = InstantiateFVM(dataEscolhida, veterinarioEscolhido);

            if(fVM.HorariosFiltrados.Count <= 0)
            {
                TempData["erro"] = "Desculpe, parece que não temos mais vagas nesse dia!";
                return RedirectToAction("Data");
            }

            CreateChoosenTempData(veterinarioEscolhido, dataEscolhida);

            return View(fVM);
        }

        [HttpPost]
        public async Task<IActionResult> Formulario(FormularioViewModel fVM)
        {
            if(!ModelState.IsValid)
            {
                ModelValidation(fVM);
                return RedirectToAction("Formulario");
            }
            else if(TemConsultasPendentes() == true)
            {
                TempData["error"] = "Você tem consultas pendentes, consulte primeiro antes de criar uma nova consulta!";
                return RedirectToAction(actionName: "SuasConsultas", controllerName: "Consultas");
            }

            try
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                _consultaRepository.CreateConsulta(fVM, user);
                await _consultaRepository.SaveChangesAsync();

                TempData["success"] = "Consulta marcada com sucesso";
                return RedirectToAction(actionName: "SuasConsultas", controllerName: "Consultas");
            }
            catch (System.Exception ex)
            {
                //Procura na mensagem se ele contém a seguinte string IX_Consultas_VeterinarioId_DataConsulta_HorarioConsulta
                var message = ex.ToString();
                if (message.Contains("IX_Consultas_VeterinarioId_DataConsulta_HorarioConsulta"))
                {
                    TempData["error"] = "Que pena! Parece que alguém agendou primeiro que você, tente outro horário.";
                    return RedirectToAction("Data");
                }
                else
                    return BadRequest();
            }
        }

        // FUNÇÕES //

        private void CreateChoosenTempData(int veterinarioEscolhido, string dataEscolhida)
        {
            var tempVetId = Newtonsoft.Json.JsonConvert.SerializeObject(veterinarioEscolhido);
            TempData["veterinarioIdEscolhido"] = tempVetId;
            TempData["dataEscolhida"] = dataEscolhida;
        }

        private string GetDataChoosenFromTemp()
        {
            return TempData["dataEscolhida"].ToString();
        }

        private int GetVetIdChoosenFromTemp()
        {
            return JsonConvert.DeserializeObject<int>(TempData["veterinarioIdEscolhido"].ToString());
        }

        private FormularioViewModel InstantiateFVM(string dataEscolhida, int veterinarioEscolhido)
        {
            var consultas = _consultaRepository.GetConsultaByDateAndVet(dataEscolhida, veterinarioEscolhido);
            var horarios = _consultaRepository.Horarios;

            var user = GetUser().Result;

            var animais = user.Animais;

            var veterinario = GetVeterinario(veterinarioEscolhido).Result;
            var horariosFiltrados = GetHorariosFiltrados(consultas, horarios);

            var formularioViewModel = new FormularioViewModel()
            {
                HorariosFiltrados = horariosFiltrados,
                Animais = animais,
                Veterinario = veterinario,
                VeterinarioId = veterinario.VeterinarioId,
                DataConsulta = dataEscolhida
            };

            return formularioViewModel;
        }

        private List<string> GetHorariosFiltrados(IEnumerable<Consulta> consultas, IEnumerable<Horario> horarios)
        {
            List<string> HorariosFiltrados = new List<string>();

            foreach (Horario horario in horarios)
            {
                HorariosFiltrados.Add(horario.Hora);
                if (consultas.LongCount() > 0)
                {
                    foreach (Consulta consulta in consultas)
                    {
                        if (horario.Hora == consulta.HorarioConsulta)
                        {
                            HorariosFiltrados.Remove(horario.Hora);
                        }
                    }
                }
            }

            return HorariosFiltrados;
        }

        private async Task<Veterinario> GetVeterinario(int vetId)
        {
            return await _consultaRepository.GetVetById(vetId);
        }

        private void ModelValidation(FormularioViewModel fVM)
        {
            if (fVM.DescricaoDoProblema == null)
            {
                TempData["descricaoValidator"] = "Descrição do problema necessário!";
            }
            else if (fVM.DescricaoDoProblema.Length < 20)
            {
                TempData["descricaoValidator"] = "Necessário mínimo de 20 caracteres!";
            }
            else
            {
                TempData["descricaoValidator"] = "Ocorreu um erro não esperado! Caso persistir faça contato com a gente!";
            }
        }

        private bool DateOnPast(string dataEscolhida)
        {
            DateTime dateConverted = Convert.ToDateTime(dataEscolhida);
            if (dateConverted < DateTime.Now.Date)
            {
                return true;
            }

            return false;
        }

        private async Task<ApplicationUser> GetUser()
        {
            return await _consultaRepository.GetUser(_userManager.GetUserId(HttpContext.User));
        }

        private bool TemConsultasPendentes()
        {
            var user = _userManager.GetUserId(User);
            
            var consultas = _consultaRepository.GetConsultasByOwnerId(user);

            if(consultas.Where(v => v.IsVerificado == false).Count() > 0)
            {
                return true;
            } 

            if(consultas.Where(a => a.IsConcluido == false).Count() > 0)
            {
                return true;
            }

            return false;
        }
    }
}
