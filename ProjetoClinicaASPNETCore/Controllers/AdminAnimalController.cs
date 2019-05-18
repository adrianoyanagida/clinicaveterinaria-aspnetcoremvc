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

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProjetoClinicaASPNETCore.Controllers
{
    [Authorize(Roles = "Administrador, Funcionario")]
    public class AdminAnimalController : Controller
    {
        private IAnimalRepository _animalRepository { get; }
        private IMapper _mapper { get; }

        public AdminAnimalController(
            IAnimalRepository animalRepository,
            IMapper mapper
            )
        {
            _animalRepository = animalRepository;
            _mapper = mapper;
        }

        public IActionResult Editar(int animalId)
        {
            var animal = _animalRepository.GetAnimalById(animalId).Result;

            if(animal == null)
            {
                TempData["error"] = "Animal não encontrado!";
                return RedirectToAction(actionName: "TodosAnimais", controllerName: "AdminAnimais");
            }

            var animalDTO = _mapper.Map<AnimalDTO>(animal);

            TempData["animalId"] = animal.AnimalId;
            return View(animalDTO);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(AnimalDTO animalDTO)
        {
            if(!ModelState.IsValid)
            {
                return View(animalDTO);
            }

            try
            {
                var animal = MapAnimalDTO(animalDTO);

                _animalRepository.Update(animal);

                await _animalRepository.SaveChangesAsync();

                TempData["success"] = "Edição feita com sucesso";
                return RedirectToAction(actionName: "TodosAnimais", controllerName: "AdminAnimais");
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        //Functions

        private int GetAnimalIdTempData()
        {
            return JsonConvert.DeserializeObject<int>(TempData["animalId"].ToString());
        }

        private Animal MapAnimalDTO(AnimalDTO animalDTO)
        {
            var animalToUpdate = _animalRepository.GetAnimalById(GetAnimalIdTempData()).Result;

            var animal = _mapper.Map(animalDTO, animalToUpdate);

            return animal;
        }
    }
}
