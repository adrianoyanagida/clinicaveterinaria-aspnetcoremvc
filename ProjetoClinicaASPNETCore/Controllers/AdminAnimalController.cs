using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using ProjetoClinicaASPNETCore.Data.DTOs;
using ProjetoClinicaASPNETCore.Data.Interfaces;
using ProjetoClinicaASPNETCore.Data.Models;
using ProjetoClinicaASPNETCore.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProjetoClinicaASPNETCore.Controllers
{
    [Authorize(Roles = "Administrador, Funcionario")]
    public class AdminAnimalController : Controller
    {
        private IAnimalRepository _animalRepository { get; }
        private IUserRepository _userRepository { get; }
        private UserManager<ApplicationUser> _userManager { get; }
        private IMapper _mapper { get; }

        public AdminAnimalController(
            IAnimalRepository animalRepository,
            IUserRepository userRepository,
            UserManager<ApplicationUser> userManager,
            IMapper mapper
            )
        {
            _userRepository = userRepository;
            _animalRepository = animalRepository;
            _userManager = userManager;
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

            animalDTO.IdDoNotMap = animal.AnimalId;
            return View(animalDTO);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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

        public IActionResult Adicionar()
        {
            var users = _userRepository.GetUsers();

            var userListViewModel = new UserListViewModel
            {
                Users = users
            };

            return View(userListViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Cadastrar(AnimalDTO animalDTO)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    var animal = _mapper.Map<Animal>(animalDTO);
                    animal.UserId = animalDTO.IdUserDoNotMap;

                    _animalRepository.Add(animal);
                    await _animalRepository.SaveChangesAsync();

                    TempData["success"] = "Animal cadastrado com sucesso";
                    return RedirectToAction(actionName: "Adicionar", controllerName: "AdminAnimal");
                }
                catch (System.Exception ex)
                {
                    return this.StatusCode(StatusCodes.Status500InternalServerError, ex);
                }
            }
            IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
            // TODO : Arrumar isso
            TempData["error"] = "Ocorreu um erro de validação";
            return RedirectToAction(actionName: "Adicionar", controllerName: "AdminAnimal");
        }

        //Functions
        private Animal MapAnimalDTO(AnimalDTO animalDTO)
        {
            var animalToUpdate = _animalRepository.GetAnimalById(animalDTO.IdDoNotMap).Result;

            var animal = _mapper.Map(animalDTO, animalToUpdate);

            return animal;
        }
    }
}
