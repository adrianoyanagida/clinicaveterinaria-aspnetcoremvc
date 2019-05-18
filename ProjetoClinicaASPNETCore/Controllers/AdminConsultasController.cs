using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjetoClinicaASPNETCore.Data.Interfaces;
using ProjetoClinicaASPNETCore.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProjetoClinicaASPNETCore.Controllers
{
    [Authorize(Roles = "Administrador, Funcionario")]
    public class AdminConsultasController : Controller
    {
        private readonly IConsultaRepository _consultaRepository;

        public AdminConsultasController(IConsultaRepository consultaRepository)
        {
            _consultaRepository = consultaRepository;
        }

        public IActionResult TodasConsultas()
        {
            var consultas = _consultaRepository.GetConsultas();

            var consultaListViewModel = new ConsultaListViewModel
            {
                Consultas = consultas
            };

            return View(consultaListViewModel);
        }

        public async Task<IActionResult> Delete(int idConsulta)
        {
            var consulta = await _consultaRepository.GetConsultaById(idConsulta);

            try
            {
                _consultaRepository.Remove(consulta);
                await _consultaRepository.SaveChangesAsync();
                TempData["success"] = "Consulta excluída com sucesso!";
                return RedirectToAction("TodasConsultas");
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }
        }
    }
}
