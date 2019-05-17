using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjetoClinicaASPNETCore.Data.Interfaces;
using ProjetoClinicaASPNETCore.Data.Models;
using ProjetoClinicaASPNETCore.ViewModels;

namespace ProjetoClinicaASPNETCore.Controllers
{
    [Authorize]
    public class AnimaisController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAnimalRepository _animalRepository;

        public AnimaisController(
            UserManager<ApplicationUser> userManager,
            IAnimalRepository animalRepository
        )
        {
            _userManager = userManager;
            _animalRepository = animalRepository;
        }

        public IActionResult SeusAnimais() {
            var userId = _userManager.GetUserId(User);
            
            var animais = _animalRepository.GetAnimaisByUserId(userId);

            var animalListViewModel = new AnimalListViewModel
            {
                Animais = animais
            };

            return View(animalListViewModel);
        }
    }
}