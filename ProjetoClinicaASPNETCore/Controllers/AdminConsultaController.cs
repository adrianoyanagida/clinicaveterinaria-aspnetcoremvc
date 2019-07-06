using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjetoClinicaASPNETCore.Data.DTOs;
using ProjetoClinicaASPNETCore.Data.Interfaces;
using ProjetoClinicaASPNETCore.Data.Models;
using ProjetoClinicaASPNETCore.ViewModels;

namespace ProjetoClinicaASPNETCore.Controllers
{
    [Authorize(Roles = "Administrador, Funcionario")]
    public class AdminConsultaController : Controller
    {
        private readonly IConsultaRepository _consultaRepository;
        private readonly IAnimalRepository _animalRepository;
        private readonly IVeterinarioRepository _veterinarioRepository;
        private readonly IVeterinarioHorarioRepository _veterinarioHorarioRepository;
        private readonly IMapper _mapper;

        public AdminConsultaController(
            IConsultaRepository consultaRepository,
            IAnimalRepository animalRepository,
            IVeterinarioRepository veterinarioRepository,
            IVeterinarioHorarioRepository veterinarioHorarioRepository,
            IMapper mapper
        )
        {
            _veterinarioRepository = veterinarioRepository;
            _consultaRepository = consultaRepository;
            _animalRepository = animalRepository;
            _veterinarioHorarioRepository = veterinarioHorarioRepository;
            _mapper = mapper;
        }

        public IActionResult Editar(int consultaId)
        {
            var consulta = _consultaRepository.GetConsultaById(consultaId).Result;

            if(consulta == null)
            {
                TempData["error"] = "Consulta não encontrada!";
                return RedirectToAction(actionName: "TodasConsultas", controllerName: "AdminConsultas");
            }

            var consultaDTO = _mapper.Map<ConsultaDTO>(consulta);

            consultaDTO.numeroDaConsulta = consulta.ConsultaId;
            return View(consultaDTO);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(ConsultaDTO consultaDTO)
        {
            if(!ModelState.IsValid)
            {
                return View(consultaDTO);
            }

            try
            {
                var consulta = MapConsultaDTO(consultaDTO);

                _consultaRepository.Update(consulta);

                await _consultaRepository.SaveChangesAsync();

                TempData["success"] = "Edição de consulta feita com sucesso!";
                return RedirectToAction(actionName: "TodasConsultas", controllerName: "AdminConsultas");
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        public IActionResult Adicionar()
        {
            var animais = _animalRepository.GetAllAnimais();

            var animalListViewModel = new AnimalListViewModel
            {
                Animais = animais
            };

            return View(animalListViewModel);
        }

        public IActionResult SelecionarVeterinario(int AnimalId)
        {
            var veterinarios = _veterinarioRepository.Veterinarios;
            var animal = _animalRepository.GetAnimalById(AnimalId).Result;

            if (!AnimalValidado(animal))
            {
                return RedirectToAction("Adicionar");
            }

            var adminConsultaVM = new AdminConsultaVM
            {
                Veterinarios = veterinarios,
                Animal = animal
            };

            return View(adminConsultaVM);
        }

        public IActionResult SelecionarData(int animalId, int vetId)
        {
            var veterinario = _veterinarioRepository.GetVetById(vetId).Result;
            var animal = _animalRepository.GetAnimalById(animalId).Result;

            if(!AnimalValidado(animal))
            {
                return RedirectToAction("Adicionar");
            }
            else if (!VeterinarioValidado(veterinario))
            {
                return RedirectToAction("SelecionarVeterinario", new { animalId = animalId });
            }

            var horariosPorVeterinario = _veterinarioHorarioRepository.VeterinarioHorariosById(vetId);
            List<string> diasDisponiveis = new List<string>();

            for(DateTime i = DateTime.Today; i < DateTime.Today.AddDays(14); i = i.AddDays(1))
            {
                if(i.DayOfWeek != DayOfWeek.Sunday)
                {
                    foreach (VeterinarioHorario horario in horariosPorVeterinario)
                    {
                        if (_consultaRepository.GetConsultaByDateAndVetAndTime(i.ToShortDateString(), vetId, horario.Horario.Hora).LongCount() == 0)
                        {
                            diasDisponiveis.Add(i.ToShortDateString());
                            break;
                        }
                    }
                }
            }

            if(diasDisponiveis.Count() == 0)
            {
                TempData["error"] = "Não foi encontrado nenhuma data disponível nos próximos 14 dias.";
                return RedirectToAction("SelecionarVeterinario");
            }

            var adminConsultaVM = new AdminConsultaVM
            {
                Veterinario = veterinario,
                Animal = animal,
                DiasDisponiveis = diasDisponiveis
            };

            return View(adminConsultaVM);
        }

        //
        private Consulta MapConsultaDTO(ConsultaDTO consultaDTO)
        {
            var consultaToUpdate = _consultaRepository.GetConsultaById(consultaDTO.numeroDaConsulta).Result;

            var consulta = _mapper.Map(consultaDTO, consultaToUpdate);

            return consulta;
        }

        private bool AnimalValidado(Animal animal)
        {
            if (animal == null)
            {
                TempData["error"] = "Ocorreu um erro: Animal não encontrado";
                return false;
            }
            else
                return true;
        }

        private bool VeterinarioValidado(Veterinario veterinario)
        {
            if (veterinario == null)
            {
                TempData["error"] = "Ocorreu um erro: Veterinário não encontrado";
                return false;
            }
            else
                return true;
        }
    }
}