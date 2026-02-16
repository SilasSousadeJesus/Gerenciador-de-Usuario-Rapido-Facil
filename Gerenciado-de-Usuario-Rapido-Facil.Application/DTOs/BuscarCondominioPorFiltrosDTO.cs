using Gerenciado_de_Usuario_Rapido_Facil.CrossCutting.Util.Enum;

namespace Gerenciado_de_Usuario_Rapido_Facil.Application.DTOs
{
    public class BuscarCondominioPorFiltrosDTO
    {
        public string nome { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string Cnpj { get; set; } = string.Empty;
        public string CodigoVinculacao { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public bool? ativo { get; set; } = null;
        public bool? PeriodoTeste { get; set; } = null;
        public DateTime dataCadastro { get; set; }
        public int pagina { get; set; } = 1;
        public int itensPorPagina { get; set; } = 10;
    }
}
