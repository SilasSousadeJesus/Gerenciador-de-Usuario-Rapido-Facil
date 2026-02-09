namespace Gerenciado_de_Usuario_Rapido_Facil.Application.DTOs
{
    public class BuscarCondominoPorFiltrosDTO
    {
        public string nome { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public bool? ativo { get; set; } = null;
        public DateTime dataCadastro { get; set; }
        public int pagina { get; set; } = 1;
        public int itensPorPagina { get; set; } = 10;
    }
}
