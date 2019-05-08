using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjetoClinicaASPNETCore.Data.Interfaces;
using ProjetoClinicaASPNETCore.Data.Models;
using ProjetoClinicaASPNETCore.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProjetoClinicaASPNETCore.Controllers
{
    [Authorize]
    public class ConsultasController : Controller
    {
        private readonly IConsultasRepository _consultasRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public ConsultasController(IConsultasRepository consultasRepository, UserManager<ApplicationUser> userManager)
        {
            _consultasRepository = consultasRepository;
            _userManager = userManager;
        }

        public IActionResult SuasConsultas()
        {
            var consultas = _consultasRepository.GetConsultasByOwnerId(_userManager.GetUserId(User));

            var sCVM = new SuasConsultasViewModel()
            {
                Consultas = consultas
            };

            return View(sCVM);
        }

        public async Task<IActionResult> Delete(int idConsulta)
        {
            var currentUserId = _userManager.GetUserId(User);
            var consulta = await _consultasRepository.GetConsultaById(idConsulta);

            if (consulta.UserId == currentUserId && consulta.IsVerificado == false)
            {
                try
                {
                    _consultasRepository.Remove(consulta);
                    await _consultasRepository.SaveChangesAsync();
                    TempData["success"] = "Consulta excluída com sucesso!";
                    return RedirectToAction("SuasConsultas");
                }
                catch (System.Exception)
                {
                    return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
                }
            }

            return BadRequest();
        }
    }
}
