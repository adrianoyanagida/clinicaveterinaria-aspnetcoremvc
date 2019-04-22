using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoClinicaASPNETCore.Data.Models
{
    public class ConsultaMarcada
    {
        [Key]
        public int Id { get; set; }
        public int ConsultaId { get; set; }
        public virtual Consulta Consulta { get; set; }
    }
}
