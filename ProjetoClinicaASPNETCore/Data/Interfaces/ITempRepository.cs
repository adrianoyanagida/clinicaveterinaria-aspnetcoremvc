using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoClinicaASPNETCore.Data.Interfaces
{
    public interface ITempRepository
    {
        void SetHorarioConsulta(string data, string horario);
    }
}
