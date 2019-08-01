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

            if (!AnimalChecado(animal)) { return RedirectToAction("Adicionar"); }

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

            if(!AnimalChecado(animal)) { return RedirectToAction("Adicionar"); }
            else if (!VeterinarioChecado(veterinario))
            {
                return RedirectToAction("SelecionarVeterinario", new { animalId = animalId });
            }

            var horariosPorVeterinario = _veterinarioHorarioRepository.VeterinarioHorariosById(vetId);
            List<string> diasDisponiveis = new List<string>();

            /* 
             * Funcionamento desse 'for'
             * O valor definido para esse 'for' é o dia atual, ele seguirá o loop até ter 14 dias depois do dia atual,
             * cada loop tera acréscimo de 1 dia.
             * 
             * No primeiro 'if' ele verifica se o dia em questão não é um domingo
             * 
             * No 'foreach' ele irá percorrer todos os horários do veterinário, para cada horário ele ira fazer uma requisição
             * no banco de dados, caso retorne nenhum valor, ele marca aquele dia como válido e quebra o foreach, caso retorne '1',
             * ele irá ir para o próximo loop do 'foreach'.
            */
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
                return RedirectToAction("SelecionarVeterinario", new { animalId = animalId });
            }

            var adminConsultaVM = new AdminConsultaVM
            {
                Veterinario = veterinario,
                Animal = animal,
                DiasDisponiveis = diasDisponiveis
            };

            return View(adminConsultaVM);
        }

        public IActionResult SelecionarHorario(int animalId, int vetId, string data)
        {
            var veterinario = _veterinarioRepository.GetVetById(vetId).Result;
            var animal = _animalRepository.GetAnimalById(animalId).Result;
            DateTime dataConvertida = new DateTime();

            if (!AnimalChecado(animal))
            {
                return RedirectToAction("Adicionar");
            }
            else if (!VeterinarioChecado(veterinario))
            {
                return RedirectToAction("SelecionarVeterinario", new { animalId = animalId });
            }
            else
            {
                try
                {
                    dataConvertida = Convert.ToDateTime(data);
                    if (dataConvertida.DayOfWeek == DayOfWeek.Sunday || dataConvertida > DateTime.Today.AddDays(14) || dataConvertida < DateTime.Today)
                    {
                        TempData["error"] = "Ocorreu um erro na data escolhida, caso persistir o erro entre em contato conosco.";
                        return RedirectToAction("SelecionarData", new { animalId = animalId, vetId = vetId });
                    }
                }
                catch(Exception)
                {
                    TempData["error"] = "Ocorreu um erro na data escolhida, caso persistir o erro entre em contato conosco.";
                    return RedirectToAction("SelecionarData", new { animalId = animalId, vetId = vetId });
                }
            }

            var horariosPorVeterinario = _veterinarioHorarioRepository.VeterinarioHorariosById(vetId);
            List<VeterinarioHorario> horariosDisponiveis = new List<VeterinarioHorario>();

            foreach (VeterinarioHorario horario in horariosPorVeterinario)
            {
                if (_consultaRepository.GetConsultaByDateAndVetAndTime(dataConvertida.ToShortDateString(), vetId, horario.Horario.Hora).LongCount() == 0)
                {
                    horariosDisponiveis.Add(horario);
                }
            }

            if(horariosDisponiveis.Count() == 0)
            {
                TempData["error"] = "Não foram encontrados horários disponíveis para essa data, " +
                    "provavelmente alguém marcou a última consulta do dia primeiro que você :(, " +
                    "caso ache que isto pode ser um erro, entre em contato conosco";
                return RedirectToAction("SelecionarVeterinario", new { animalId = animalId, vetId = vetId });
            }

            var adminConsultaVM = new AdminConsultaVM
            {
                Veterinario = veterinario,
                Animal = animal,
                DataEscolhida = Convert.ToDateTime(dataConvertida).ToShortDateString(),
                HorariosDisponiveis = horariosDisponiveis
            };

            return View(adminConsultaVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SelecionarHorario(AdminConsultaVM adminConsultaVM)
        {
            var veterinario = _veterinarioRepository.GetVetById(adminConsultaVM.Veterinario.VeterinarioId).Result;
            var animal = _animalRepository.GetAnimalById(adminConsultaVM.Animal.AnimalId).Result;
            DateTime dataConvertida = new DateTime();

            if (!AnimalChecado(animal))
            {
                return RedirectToAction("Adicionar");
            }
            else if (!VeterinarioChecado(veterinario))
            {
                return RedirectToAction("SelecionarVeterinario", new { animalId = adminConsultaVM.Animal.AnimalId });
            }
            else
            {
                try
                {
                    dataConvertida = Convert.ToDateTime(adminConsultaVM.DataEscolhida);
                    if (dataConvertida.DayOfWeek == DayOfWeek.Sunday || dataConvertida > DateTime.Today.AddDays(14) || dataConvertida < DateTime.Today)
                    {
                        TempData["error"] = "Ocorreu um erro na data escolhida, caso persistir o erro entre em contato conosco.";
                        return RedirectToAction("SelecionarData", new { animalId = adminConsultaVM.Animal.AnimalId, vetId = adminConsultaVM.Veterinario.VeterinarioId });
                    }
                }
                catch (Exception)
                {
                    TempData["error"] = "Ocorreu um erro na data escolhida, caso persistir o erro entre em contato conosco.";
                    return RedirectToAction("SelecionarData", new { animalId = adminConsultaVM.Animal.AnimalId, vetId = adminConsultaVM.Veterinario.VeterinarioId });
                }
            }

            return BadRequest();
        }

        // FUNCTIONS
        private Consulta MapConsultaDTO(ConsultaDTO consultaDTO)
        {
            // Pega a consulta original do banco para mapear
            var consultaToUpdate = _consultaRepository.GetConsultaById(consultaDTO.numeroDaConsulta).Result;
            // Mapeia a consulta nova trocando as informações da original
            var consulta = _mapper.Map(consultaDTO, consultaToUpdate);
            return consulta;
        }

        private bool AnimalChecado(Animal animal)
        {
            if (animal == null)
            {
                TempData["error"] = "Ocorreu um erro: Animal não encontrado";
                return false;
            }
            else
                return true;
        }

        private bool VeterinarioChecado(Veterinario veterinario)
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