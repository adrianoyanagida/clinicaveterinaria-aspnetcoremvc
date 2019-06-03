using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjetoClinicaASPNETCore.Data.Interfaces;
using ProjetoClinicaASPNETCore.Data.Models;
using ProjetoClinicaASPNETCore.ViewModels;
using System.Threading.Tasks;

namespace ProjetoClinicaASPNETCore.Controllers
{
    [Authorize(Roles = "Administrador, Funcionario")]
    public class AdminAnimaisController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAnimalRepository _animalRepository;

        public AdminAnimaisController(
            UserManager<ApplicationUser> userManager,
            IAnimalRepository animalRepository
        )
        {
            _userManager = userManager;
            _animalRepository = animalRepository;
        }

        public IActionResult TodosAnimais() {
            var animais = _animalRepository.GetAllAnimais();

            var animalListViewModel = new AnimalListViewModel
            {
                Animais = animais
            };

            return View(animalListViewModel);
        }

        public async Task<IActionResult> Delete(int idAnimal)
        {
            try
            {
                var animal = _animalRepository.GetAnimalById(idAnimal).Result;

                _animalRepository.Remove(animal);
                await _animalRepository.SaveChangesAsync();
                TempData["success"] = "Animal exclu�do com sucesso!";
                return RedirectToAction("TodosAnimais");
            }
            catch(System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }
        }
    }
}