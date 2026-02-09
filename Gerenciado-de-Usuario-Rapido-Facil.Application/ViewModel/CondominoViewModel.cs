namespace Gerenciado_de_Usuario_Rapido_Facil.Application.ViewModel
{
    public class CondominoViewModel
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string CnpjCpf { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public bool Ativo { get; set; } = true;

        public string Rua { get; set; } = string.Empty;
        public string Bairro { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string Cep { get; set; } = string.Empty;


        public bool Vinculado { get; set; }
        public string CodigoVinculacao { get; set; } = string.Empty;
        public Guid? CondominioId { get; set; }

        public CondominioViewModel? Condominio { get; set; }
    }
}
