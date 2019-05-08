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
        private readonly IAnimaisRepository _animaisRepository;

        public AnimaisController(
            UserManager<ApplicationUser> userManager,
            IAnimaisRepository animaisRepository
        )
        {
            _userManager = userManager;
            _animaisRepository = animaisRepository;
        }

        public IActionResult SeusAnimais() {
            var userId = _userManager.GetUserId(User);
            
            var animais = _animaisRepository.GetAnimaisById(userId);

            var animalListViewModel = new AnimalListViewModel
            {
                Animais = animais
            };

            return View(animalListViewModel);
        }
    }
}