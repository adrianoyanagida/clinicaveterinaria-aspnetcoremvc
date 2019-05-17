using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoClinicaASPNETCore.Data.Models
{
    public class VeterinarioHorario
    {
        public int VeterinarioId { get; set; }
        public Veterinario Veterinario { get; set; }

        public int HorarioId { get; set; }
        public Horario Horario { get; set; }
    }
}
