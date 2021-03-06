﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjetoClinicaASPNETCore.Data.DTOs;
using ProjetoClinicaASPNETCore.Data.Interfaces;
using ProjetoClinicaASPNETCore.Data.Models;
using ProjetoClinicaASPNETCore.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProjetoClinicaASPNETCore.Controllers
{
    [Authorize(Roles = "Administrador, Funcionario")]
    public class AdminUsuarioController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public AdminUsuarioController(UserManager<ApplicationUser> userManager, IUserRepository userRepository, IMapper mapper)
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public IActionResult Editar(string usuarioId)
        {
            var usuario = _userRepository.GetUser(usuarioId).Result;

            if(usuario == null)
            {
                TempData["error"] = "Usuário não encontrado!";
                return RedirectToAction(actionName: "TodosUsuarios", controllerName: "AdminUsuarios");
            }

            var usuarioDTO = _mapper.Map<UsuarioDTO>(usuario);

            usuarioDTO.IsCliente = _userManager.IsInRoleAsync(usuario, "Cliente").Result;
            usuarioDTO.IsAdministrador = _userManager.IsInRoleAsync(usuario, "Administrador").Result;
            usuarioDTO.IsFuncionario = _userManager.IsInRoleAsync(usuario, "Funcionario").Result;
            usuarioDTO.IdDoNotMap = usuario.Id;
            return View(usuarioDTO);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(UsuarioDTO usuarioDTO)
        {
            var validator = new CPFValidator();
            if(usuarioDTO.CPF != null)
            {
                if(!validator.IsCpf(usuarioDTO.CPF))
                {
                    ModelState.AddModelError("CPF", "O CPF fornecido é inválido!");
                }else
                    usuarioDTO.CPF = new String(usuarioDTO.CPF.Where(Char.IsDigit).ToArray());
            }

            if(!ModelState.IsValid)
            {
                return View(usuarioDTO);
            }

            try
            {
                var usuario = MapUserDTO(usuarioDTO);
                _userRepository.Update(usuario);
                await _userManager.UpdateNormalizedEmailAsync(usuario);
                await _userManager.UpdateNormalizedUserNameAsync(usuario);


                if(usuarioDTO.IsAdministrador == true)
                {
                    await _userManager.AddToRoleAsync(usuario, "Administrador");
                }
                else
                {
                    await _userManager.RemoveFromRoleAsync(usuario, "Administrador");
                }
                if(usuarioDTO.IsCliente == true)
                {
                    await _userManager.AddToRoleAsync(usuario, "Cliente");
                }
                else
                {
                    await _userManager.RemoveFromRoleAsync(usuario, "Cliente");
                }
                if(usuarioDTO.IsFuncionario == true)
                {
                    await _userManager.AddToRoleAsync(usuario, "Funcionario");
                }
                else
                {
                    await _userManager.RemoveFromRoleAsync(usuario, "Funcionario");
                }
                
                await _userRepository.SaveChangesAsync();
                TempData["success"] = "Edição de usuário feita com sucesso!";
                return RedirectToAction(actionName: "TodosUsuarios", controllerName: "AdminUsuarios");
            }
            catch (System.Exception ex)
            {
                var message = ex.ToString();
                if (message.Contains("IX_AspNetUsers_Email"))
                    ModelState.AddModelError("Email", "E-mail já cadastrado, tente outro!");
                else 
                if(message.Contains("CPF"))
                    ModelState.AddModelError("CPF", "CPF já cadastrado");
                else
                if(message.Contains("UserName"))
                    ModelState.AddModelError("UserName", "Nome de usuário já cadastrado");
                else
                return this.StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
            return View(usuarioDTO);
        }

        // FUNCOES
        private ApplicationUser MapUserDTO(UsuarioDTO usuarioDTO)
        {
            var usuarioToUpdate = _userRepository.GetUser(usuarioDTO.IdDoNotMap).Result;

            var usuario = _mapper.Map(usuarioDTO, usuarioToUpdate);

            return usuario;
        }
    }
}
