using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoClinicaASPNETCore.Data.DTOs
{
    public class AnimalDTO
    {
        [Required]
        public int IdDoNotMap { get; set; }
        
        [Required(ErrorMessage = "É necessário preencher o campo 'Nome do Animal'!")]
        [Display(Name = "Nome do Animal *")]
        public string AnimalNome { get; set; }

        [Required(ErrorMessage = "É necessário preencher o campo 'Tipo do Animal'!")]
        [Display(Name = "Tipo do Animal *")]
        public string AnimalTipo { get; set; }

        [Display(Name = "Raça do Animal")]
        public string AnimalRaca { get; set; }

        [Display(Name = "Data de Nascimento")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public string AnimalDataDeNascimento { get; set; }

        [Display(Name = "Alergias? Quais?")]
        public string Alergico { get; set; }
    }
}
