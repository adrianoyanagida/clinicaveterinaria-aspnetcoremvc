﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoClinicaASPNETCore.Data.Models
{
    public class Horario
    {
        public int HorarioId { get; set; }

        [Required]
        public string Hora { get; set; }

        public List<VeterinarioHorario> VeterinarioHorarios { get; set; }
    }
}
