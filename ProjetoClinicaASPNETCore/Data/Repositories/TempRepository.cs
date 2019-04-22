using ProjetoClinicaASPNETCore.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoClinicaASPNETCore.Data.Repositories
{
    public class TempRepository : ITempRepository
    {
        public static string DataEscolhida { get; set; }
        public static string HoraEscolhida { get; set; }

        public void SetHorarioConsulta(string data, string horario)
        {
            DataEscolhida = data;
            HoraEscolhida = horario;
        }
    }
}
