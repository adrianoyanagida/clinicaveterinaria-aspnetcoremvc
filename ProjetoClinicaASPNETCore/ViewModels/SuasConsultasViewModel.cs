using ProjetoClinicaASPNETCore.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoClinicaASPNETCore.ViewModels
{
    public class SuasConsultasViewModel
    {
        public IEnumerable<Consulta> Consultas { get; set; }
    }
}
