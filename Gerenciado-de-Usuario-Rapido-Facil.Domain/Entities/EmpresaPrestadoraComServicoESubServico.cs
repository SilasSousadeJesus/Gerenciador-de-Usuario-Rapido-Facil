using Gerenciado_de_Usuario_Rapido_Facil.CrossCutting.Util.Enum;

namespace Gerenciado_de_Usuario_Rapido_Facil.Domain.Entities
{
    public class EmpresaPrestadoraComServicoESubServico
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Nome { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public string CnpjCpf { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Logo { get; set; } = string.Empty;
        public bool Ativo { get; set; } = true;
        public string Rua { get; set; } = string.Empty;
        public string Bairro { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string Cep { get; set; } = string.Empty;
        public EAnaliseIdoneidade AnaliseIdoneidade { get; set; } = EAnaliseIdoneidade.NaoAnalisada;
        public bool Idoneo{ get; set; } = false;
        public List<Servico> Servicos { get; set; }
    }
}
