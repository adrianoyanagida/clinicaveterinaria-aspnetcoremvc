namespace ProjetoClinicaASPNETCore.Data.DTOs
{
    public class ConsultaDTO
    {
        public string ValorConsulta { get; set; }
        public string Diagnostico { get; set; }
        public bool IsVerificado { get; set; }
        public bool IsConcluido { get; set;}

        // N�o mapear
        public int numeroDaConsulta { get; set; }
    }
}