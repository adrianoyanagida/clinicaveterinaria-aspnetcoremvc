using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjetoClinicaASPNETCore.Data.DTOs;
using ProjetoClinicaASPNETCore.Data.Interfaces;
using ProjetoClinicaASPNETCore.Data.Models;

namespace ProjetoClinicaASPNETCore.Controllers
{
    [Authorize(Roles = "Administrador, Funcionario")]
    public class AdminConsultaController : Controller
    {
        private readonly IConsultaRepository _consultaRepository;
        private readonly IMapper _mapper;

        public AdminConsultaController(
            IConsultaRepository consultaRepository,
            IMapper mapper
        )
        {
            _consultaRepository = consultaRepository;
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

            TempData["consultaId"] = consulta.ConsultaId;
            return View(consultaDTO);
        }

        [HttpPost]
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

        //
        private int GetConsultaIdTempData()
        {
            return JsonConvert.DeserializeObject<int>(TempData["consultaId"].ToString());
        }

        private Consulta MapConsultaDTO(ConsultaDTO consultaDTO)
        {
            var consultaToUpdate = _consultaRepository.GetConsultaById(GetConsultaIdTempData()).Result;

            var consulta = _mapper.Map(consultaDTO, consultaToUpdate);

            return consulta;
        } 
    }
}