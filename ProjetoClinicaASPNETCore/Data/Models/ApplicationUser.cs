using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ProjetoClinicaASPNETCore.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string DataDeNascimento { get; set; }
        public string NomeCompleto { get; set; }
        public string CPF { get; set; }

        public List<Animal> Animais { get; set; }
        public List<Consulta> Consultas { get; set; }
    }
}
