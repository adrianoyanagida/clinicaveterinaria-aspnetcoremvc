using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoClinicaASPNETCore.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "É necessário preencher o campo 'Usuário'!")]
        [Display(Name = "Nome de Usuário*")]
        public string NomeDeUsuario { get; set; }

        [Required(ErrorMessage = "É necessário preencher o campo 'Senha'!")]
        [Display(Name = "Senha*")]
        [DataType(DataType.Password)]
        public string Senha { get; set; }

        [Required(ErrorMessage = "É necessário preencher o campo 'Nome Completo'!")]
        [Display(Name = "Nome Completo*")]
        public string NomeCompleto { get; set; }

        [Display(Name = "Data de Nascimento*")]
        [Required(ErrorMessage = "É necessário preencher o campo 'Data de Nascimento'!")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public string DataDeNascimento { get; set; }

        [Required(ErrorMessage = "É necessário preencher o campo 'E-mail'!")]
        [Display(Name = "E-mail*")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "É necessário preencher o campo 'CPF'!")]
        [Display(Name = "CPF*")]
        public string CPF { get; set; }

        [Required(ErrorMessage = "É necessário preencher o campo 'Telefone'!")]
        [Display(Name = "Telefone*")]
        [DataType(DataType.PhoneNumber)]
        public string Telefone { get; set; }

        public string ReturnUrl { get; set; }
    }
}
