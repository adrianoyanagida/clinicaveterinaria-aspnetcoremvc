﻿using System;
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
    public class AdminUsuariosController : Controller
    {
        private readonly IUserRepository _userRepository;

        public AdminUsuariosController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IActionResult TodosUsuarios()
        {
            var users = _userRepository.GetUsers();

            var userListViewModel = new UserListViewModel
            {
                Users = users
            };

            return View(userListViewModel);
        }

        public async Task<IActionResult> Delete(string idUsuario)
        {
            var user = _userRepository.GetUser(idUsuario).Result;

            try
            {
                _userRepository.Remove(user);
                await _userRepository.SaveChangesAsync();
                TempData["success"] = "Usuário excluído com sucesso!";
                return RedirectToAction("TodosUsuarios");
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }
        }
    }
}
