using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoClinicaASPNETCore.Data.DTOs
{
    public class UsuarioDTO
    {   
        [Required]
        public string IdDoNotMap { get; set; }

        [Required(ErrorMessage = "É necessário preencher o campo 'Usuário'!")]
        [Display(Name = "Nome de Usuário*")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "É necessário preencher o campo 'E-mail'!")]
        [Display(Name = "E-mail*")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Nome Completo")]
        public string NomeCompleto { get; set; }

        [Display(Name = "Telefone")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Display(Name = "CPF")]
        public string CPF { get; set; }

        [Display(Name = "Data de Nascimento")]
        [DataType(DataType.Date)]
        public string DataDeNascimento { get; set; }
    }
}
