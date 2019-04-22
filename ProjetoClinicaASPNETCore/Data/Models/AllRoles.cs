using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoClinicaASPNETCore.Data.Models
{
    public class AllRoles
    {
        public class Administrador
        {
            public string funcao = "Administrador";
            public string descricao = "A chave mestre do aplicativo";
        }

        public class Funcionario
        {
            public string funcao = "Funcionario";
            public string descricao = "Tem certo controle sobre o aplicativo";
        }

        public class Cliente
        {
            public string funcao = "Cliente";
            public string descricao = "Cliente do aplicativo";
        }

        public string GetDefaultRole()
        {
            return "Cliente";
        }
    }
}
