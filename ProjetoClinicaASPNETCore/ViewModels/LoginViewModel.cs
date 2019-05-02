using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoClinicaASPNETCore.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "É necessário preencher o campo 'Nome de Usuário/E-mail!'")]
        [Display(Name = "Usuário ou E-mail")]
        public string UsuarioOuEmail { get; set; }

        [Required(ErrorMessage = "É necessário preencher o campo 'Senha!'")]
        [DataType(DataType.Password)]
        public string Senha { get; set; }
    }
}
