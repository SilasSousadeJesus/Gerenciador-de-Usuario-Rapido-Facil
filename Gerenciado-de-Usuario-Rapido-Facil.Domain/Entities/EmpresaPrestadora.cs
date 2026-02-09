namespace Gerenciado_de_Usuario_Rapido_Facil.Domain.Entities
{
    public class EmpresaPrestadora 
    {
        public EmpresaPrestadora() { }
        public EmpresaPrestadora(
            string nome, 
            string senha, 
            string cnpjCpf, 
            string email, 
            string logo,
            bool ativo, 
            string rua, 
            string bairro, 
            string cidade, 
            string estado,
            string cep
            ) { 
        
            Nome = nome;
            Senha = senha;
            CnpjCpf = cnpjCpf;
            Email = email;
            Logo = logo;
            Ativo = ativo;
            Rua = rua;
            Bairro = bairro;
            Cidade = cidade;
            Estado = estado;
            Cep = cep;
            EmpresaPrestadoraServicoSubtipos = [];
        }

        public Guid Id { get; set; } = Guid.NewGuid();
        public string Nome { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public string CnpjCpf { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Logo { get; set; } = string.Empty;
        public bool Ativo { get; set; }
        public string Rua { get; set; } = string.Empty;
        public string Bairro { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string Cep { get; set; } = string.Empty;
        public bool AceitouTermosDeUso { get; set; } = false;
        public DateTime DataCadastro { get; set; } = DateTime.Now;
        public List<EmpresaPrestadoraServicoSubtipo> EmpresaPrestadoraServicoSubtipos { get; set; } = new List<EmpresaPrestadoraServicoSubtipo>();

        public void PreencherComIdOsServicos() {
            if (EmpresaPrestadoraServicoSubtipos.Any())
            {
                foreach (var item in EmpresaPrestadoraServicoSubtipos)
                {
                    item.EmpresaPrestadoraId = this.Id;
                }
            }
        }

        public void EditarEmpresaPrestadora(EmpresaPrestadora empresaPrestadora) {
            if (
                !string.IsNullOrWhiteSpace(empresaPrestadora.Cep) &&
                !string.Equals(Cep, empresaPrestadora.Cep, StringComparison.OrdinalIgnoreCase))
            {
                Cep = empresaPrestadora.Cep;
            }
            if (!string.IsNullOrWhiteSpace(empresaPrestadora.Nome) &&
                !string.Equals(Nome, empresaPrestadora.Nome, StringComparison.OrdinalIgnoreCase))
            {
                Nome = empresaPrestadora.Nome;
            }
            if (
                !string.IsNullOrWhiteSpace(empresaPrestadora.CnpjCpf) &&
                !string.Equals(CnpjCpf, empresaPrestadora.CnpjCpf, StringComparison.OrdinalIgnoreCase))
            {
                CnpjCpf = empresaPrestadora.CnpjCpf;
            }
            if (
                !string.IsNullOrWhiteSpace(empresaPrestadora.Email) &&
                !string.Equals(Email, empresaPrestadora.Email, StringComparison.OrdinalIgnoreCase))
            {
                Email = empresaPrestadora.Email;
            }
            if (
                empresaPrestadora.Ativo != null &&
                Ativo != empresaPrestadora.Ativo)
            {
                Ativo = empresaPrestadora.Ativo;
            }
            if (
                 !string.IsNullOrWhiteSpace(empresaPrestadora.Rua) &&
                !string.Equals(Rua, empresaPrestadora.Rua, StringComparison.OrdinalIgnoreCase))
            {
                Rua = empresaPrestadora.Rua;
            }
            if (
                 !string.IsNullOrWhiteSpace(empresaPrestadora.Bairro) &&
                !string.Equals(Bairro, empresaPrestadora.Bairro, StringComparison.OrdinalIgnoreCase))
            {
                Bairro = empresaPrestadora.Bairro;
            }
            if (
                !string.IsNullOrWhiteSpace(empresaPrestadora.Cidade) &&
                !string.Equals(Cidade, empresaPrestadora.Cidade, StringComparison.OrdinalIgnoreCase))
            {
                Cidade = empresaPrestadora.Cidade;
            }
            if (
                !string.IsNullOrWhiteSpace(empresaPrestadora.Estado) &&
                !string.Equals(Estado, empresaPrestadora.Estado, StringComparison.OrdinalIgnoreCase))
            {
                Estado = empresaPrestadora.Estado;
            }
            if (
                !string.IsNullOrWhiteSpace(empresaPrestadora.Logo) &&
                !string.Equals(Logo, empresaPrestadora.Logo, StringComparison.OrdinalIgnoreCase))
            {
                Logo = empresaPrestadora.Logo;
            }

            if (
                !string.IsNullOrWhiteSpace(empresaPrestadora.Senha) &&
                !string.Equals(Senha, empresaPrestadora.Senha, StringComparison.OrdinalIgnoreCase))
            {
                Senha = empresaPrestadora.Senha;
            }
        }
    }
}


// criar classe serviço e classe serviço subtipo, criar criação automatica desse serviços quandoa aplicação for iniciada
// criar serviço de idoneidade
// criar campo pra validar se a empresa foi validada pelo serviço de idoneidade