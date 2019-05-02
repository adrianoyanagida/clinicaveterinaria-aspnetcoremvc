using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjetoClinicaASPNETCore.Data.Models;
using ProjetoClinicaASPNETCore.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoClinicaASPNETCore.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if(!ModelState.IsValid)
            {
                return View(loginViewModel);
            }

            var userInput = loginViewModel.UsuarioOuEmail;
            //Verifica se tem @ no userInput
            if (userInput.IndexOf('@') > -1)
            {
                //Procura o usuario pelo Email
                var user = await _userManager.FindByEmailAsync(userInput);
                if(user != null)
                {
                    //Faz login do usuario passando os dados de user
                    var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Senha, false, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            else
            {
                //Procura o usuario pelo UserName
                var user = await _userManager.FindByNameAsync(loginViewModel.UsuarioOuEmail);
                if (user != null)
                {
                    //Faz login do usuario passando os dados de user
                    var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Senha, false, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            ModelState.AddModelError("", "Nome de usuário ou senha incorreto!");
            return View(loginViewModel);
        }

        public IActionResult Register() => View();

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if(ModelState.IsValid)
            {
                //Verifica o CPF se é válido ou não
                var cpf = new CPFValidator();
                if(!cpf.IsCpf(registerViewModel.CPF))
                {
                    ModelState.AddModelError("", "O CPF fornecido é inválido!");
                    return View(registerViewModel);
                }

                //Caso for válido entra nesse try catch
                try
                {
                    //É preechido o ApplicationUser com os valores da nossa View
                    var user = new ApplicationUser()
                    {
                        UserName = registerViewModel.NomeDeUsuario,
                        NomeCompleto = registerViewModel.NomeCompleto,
                        DataDeNascimento = registerViewModel.DataDeNascimento,
                        Email = registerViewModel.Email,
                        CPF = registerViewModel.CPF,
                        PhoneNumber = registerViewModel.Telefone
                    };

                    //Cria uma variável result e passa os valores para o banco, criando um novo usuario
                    var result = await _userManager.CreateAsync(user, registerViewModel.Senha);

                    if (result.Succeeded)
                    {
                        var role = new AllRoles();
                        var resultRole = await _userManager.AddToRoleAsync(user, role.GetDefaultRole());
                        if (resultRole.Succeeded)
                            return RedirectToAction(actionName: "Index", controllerName: "Home");
                    }
                    else
                    {
                        //Mostra erros padrão como exemplo: A senha é menor que 4 linhas
                        foreach (var err in result.Errors)
                        {
                            ModelState.AddModelError("", $"{err.Description}");
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    //Procura na mensagem se ele contém a seguinte string IX_AspNetUsers_Email
                    var message = ex.ToString();
                    if (message.Contains("IX_AspNetUsers_Email"))
                    {
                        ModelState.AddModelError("", "E-mail já cadastrado, tente outro!");
                    }
                    else
                    return BadRequest();
                }
            }
            return View(registerViewModel);
        }

        public ViewResult LoggedIn() => View();

        public ViewResult AccessDenied() => View();

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
