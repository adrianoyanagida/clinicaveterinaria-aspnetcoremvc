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

        [HttpPost]
        public IActionResult Data(ConsultaViewModel cVM)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Data");
            }
            CreateChoosenTempData(cVM.VeterinarioId, cVM.DataConsulta);

            return RedirectToAction("Formulario");
        }

        public IActionResult Formulario()
        {
            if (TempData["VeterinarioIdEscolhido"] == null || TempData["DataEscolhida"] == null)
            {
                return RedirectToAction("Data");
            }

            var dataEscolhida = GetDataChoosenFromTemp();
            var veterinarioEscolhido = GetVetIdChoosenFromTemp();

            var fVM = instantiateFVM(dataEscolhida, veterinarioEscolhido);

            if(fVM.HorariosFiltrados.Count <= 0)
            {
                TempData["Erro"] = "Desculpe, parece que não temos mais vagas nesse dia!";
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

            try
            {
                var currentUserId = _userManager.GetUserId(User);
                _consultaRepository.CreateConsulta(fVM, currentUserId);
                await _consultaRepository.SaveChangesAsync();

                return RedirectToAction("Concluido");
            }
            catch (System.Exception ex)
            {
                //Procura na mensagem se ele contém a seguinte string IX_Consultas_VeterinarioId_DataConsulta_HorarioConsulta
                var message = ex.ToString();
                if (message.Contains("IX_Consultas_VeterinarioId_DataConsulta_HorarioConsulta"))
                {
                    TempData["Erro"] = "Que pena! Parece que alguém agendou primeiro que você, tente outro horário.";
                    return RedirectToAction("Parte1");
                }
                else
                    return BadRequest();
            }
        }

        public IActionResult Erro() => View();

        public IActionResult Concluido() => View();

        // FUNÇÕES //

        private void CreateChoosenTempData(int veterinarioEscolhido, string dataEscolhida)
        {
            var tempVetId = Newtonsoft.Json.JsonConvert.SerializeObject(veterinarioEscolhido);
            TempData["VeterinarioIdEscolhido"] = tempVetId;
            TempData["DataEscolhida"] = dataEscolhida;
        }

        private string GetDataChoosenFromTemp()
        {
            return TempData["DataEscolhida"].ToString();
        }

        private int GetVetIdChoosenFromTemp()
        {
            return JsonConvert.DeserializeObject<int>(TempData["VeterinarioIdEscolhido"].ToString());
        }

        private FormularioViewModel instantiateFVM(string dataEscolhida, int veterinarioEscolhido)
        {
            var consultas = _consultaRepository.GetConsultaByDateAndVet(dataEscolhida, veterinarioEscolhido);
            var horarios = _consultaRepository.Horarios;
            var animais = _consultaRepository.GetAnimaisByOwnerId(_userManager.GetUserId(User));

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
                TempData["DescricaoValidator"] = "Descrição do problema necessário!";
            }
            else if (fVM.DescricaoDoProblema.Length < 30)
            {
                TempData["DescricaoValidator"] = "Necessário mínimo de 20 caracteres!";
            }
            else
            {
                TempData["DescricaoValidator"] = "Ocorreu um erro não esperado! Caso persistir faça contato com a gente!";
            }
        }
    }
}
