namespace Gerenciado_de_Usuario_Papido_Facil.Application.DTOs
{
    public class BuscarPrestadorPorFiltrosDTO
    {
        public string nome { get; set; } = string.Empty;
        public string cidade { get; set; } = string.Empty;
        public string estado { get; set; } = string.Empty;
        public int pagina { get; set; } = 1;
        public int itensPorPagina { get; set; } = 10;
        public Guid servicoId { get; set; } = Guid.Empty;
        public List<Guid> servicoSubtipoIds { get; set; } = new List<Guid>();
    }
}