using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjetoClinicaASPNETCore.Data.DTOs;
using ProjetoClinicaASPNETCore.Data.Models;
using ProjetoClinicaASPNETCore.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ProjetoClinicaASPNETCore.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IMapper mapper
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
        }

        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if(!ModelState.IsValid)
            {
                return View(loginViewModel);
            }

            try
            {
                var user = new ApplicationUser();
                user = await _userManager.FindByNameAsync(loginViewModel.UsuarioOuEmail);
                if(user == null)
                {
                    user = await _userManager.FindByEmailAsync(loginViewModel.UsuarioOuEmail);
                    if (user == null)
                    {
                        TempData["error"] = "Usuário/E-mail ou senha incorreto!";
                        return View(loginViewModel);
                    }
                }

                var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Senha, false, false);
                if(result.Succeeded)
                {
                    TempData["success"] = "Logado com sucesso";
                    return RedirectToAction("Index", "Home");
                }

                TempData["error"] = "Usuário/E-mail ou senha incorreto!";
                return View(loginViewModel);
            }
            catch
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco falhou");
            }
        }

        public IActionResult Register() => View();

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(ApplicationUserDTO applicationUserDTO)
        {
            var validator = new CPFValidator();
            if (!validator.IsCpf(applicationUserDTO.CPF))
            {
                ModelState.AddModelError("CPF", "O CPF fornecido é inválido!");
            }

            if (!ModelState.IsValid)
            {
                return View(applicationUserDTO);
            }

            applicationUserDTO.CPF = new String(applicationUserDTO.CPF.Where(Char.IsDigit).ToArray());

            try
            {
                var user = _mapper.Map<ApplicationUser>(applicationUserDTO);

                //Cria uma variável result e passa os valores para o banco, criando um novo usuario
                var result = await _userManager.CreateAsync(user, applicationUserDTO.Senha);

                if (result.Succeeded)
                {
                    var resultRole = await _userManager.AddToRoleAsync(user, AllRoles.GetDefaultRole());
                    if (resultRole.Succeeded)
                    {
                        TempData["success"] = "Registro feito com sucesso";
                        return RedirectToAction(actionName: "Index", controllerName: "Home");
                    }

                    TempData["success"] = "Registro feito, porém falha em adicionar uma função ao usuário, caso persista, contate o suporte.";
                    return RedirectToAction(actionName: "Index", controllerName: "Home");
                }
                else
                {
                    foreach (var err in result.Errors)
                    {
                        if (err.Description.Contains("User name"))
                            ModelState.AddModelError("UserName", "Nome de usuário indisponível");
                        if (err.Code == "PasswordTooShort")
                            ModelState.AddModelError("Senha", "Tamanho mínimo de 4 caracteres");
                    }
                }
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
                    return this.StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
            return View(applicationUserDTO);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            TempData["success"] = "Saiu com sucesso";
            return RedirectToAction("Index", "Home");
        }
    }
}
