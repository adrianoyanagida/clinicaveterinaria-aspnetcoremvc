using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjetoClinicaASPNETCore.Data;
using ProjetoClinicaASPNETCore.Data.DTOs;
using ProjetoClinicaASPNETCore.Data.Interfaces;
using ProjetoClinicaASPNETCore.Data.Models;
using ProjetoClinicaASPNETCore.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProjetoClinicaASPNETCore.Controllers
{
    [Authorize]
    public class AnimalController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly IAnimalRepository _animalRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public AnimalController(
            AppDbContext appDbContext,
            IAnimalRepository animalRepository,
            UserManager<ApplicationUser> userManager,
            IMapper mapper
            )
        {
            _appDbContext = appDbContext;
            _animalRepository = animalRepository;
            _userManager = userManager;
            _mapper = mapper;
        }

        public IActionResult Cadastro() => View();

        [HttpPost]
        public async Task<IActionResult> Cadastro(AnimalDTO animalDTO)
        {
            if(ModelState.IsValid)
            {
                var animal = _mapper.Map<Animal>(animalDTO);
                var userId = _userManager.GetUserId(User);

                _animalRepository.RegisterAnimal(animal, userId);
                await _appDbContext.SaveChangesAsync(); 
                return RedirectToAction("CadastroCompleto");
            }

            return View(animalDTO);
        }

        public IActionResult CadastroCompleto() => View();

        [Authorize(Roles = "Administrador, Funcionario")]
        public IActionResult ListaTodos()
        {
            IEnumerable<Animal> allAnimais;

            allAnimais = _animalRepository.AllAnimais.OrderBy(n => n.AnimalNome);

            var animalListViewModel = new AnimalListViewModel
            {
                AllAnimais = allAnimais
            };

            return View(animalListViewModel);
        }

        public IActionResult Lista()
        {
            string currentUserId = _userManager.GetUserId(User);
            IEnumerable<Animal> animais;

            if (string.IsNullOrEmpty(currentUserId))
            {
                return BadRequest();
            }
            else
            {
                animais = _animalRepository.Animais.Where(p => p.UserId.Equals(currentUserId)).OrderBy(p => p.AnimalNome);
            }

            var animalListViewModel = new AnimalListViewModel
            {
                Animais = animais
            };

            return View(animalListViewModel);
        }
    }
}
