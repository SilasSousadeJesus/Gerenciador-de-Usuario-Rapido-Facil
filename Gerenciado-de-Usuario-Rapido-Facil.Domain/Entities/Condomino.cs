using System.ComponentModel.DataAnnotations.Schema;

namespace Gerenciado_de_Usuario_Rapido_Facil.Domain.Entities
{
    public class Condomino
    {
        public Condomino() { }
        public Condomino(
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
            string cep,
            string apartamento
            )
        {
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
            Apartamento = apartamento;
        }

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
        public string Logo { get; set; } = string.Empty;
        public string Cep { get; set; } = string.Empty;
        public string Apartamento { get; set; } = string.Empty;
        public DateTime DataCadastro { get; set; } = DateTime.Now;
        public bool AceitouTermosDeUso { get; set; } = false;

        public bool Vinculado { get; set; } = false;
        public string CodigoVinculacao { get; set; } = string.Empty;
        public bool PlanoValido { get; set; } = false;

        [ForeignKey("CondominioId")]
        public Guid? CondominioId { get; set; }

        public void AtualizarCondomino(Condomino novoCondominio)
        {
            //if (!string.Equals(Cep, novoCondominio.Cep, StringComparison.OrdinalIgnoreCase))
            //{
            //    Cep = novoCondominio.Cep;
            //}
            if (!string.Equals(Nome, novoCondominio.Nome, StringComparison.OrdinalIgnoreCase))
            {
                Nome = novoCondominio.Nome;
            }
            if (!string.Equals(CnpjCpf, novoCondominio.CnpjCpf, StringComparison.OrdinalIgnoreCase))
            {
                CnpjCpf = novoCondominio.CnpjCpf;
            }
            if (!string.Equals(Email, novoCondominio.Email, StringComparison.OrdinalIgnoreCase))
            {
                Email = novoCondominio.Email;
            }
            if (Ativo != novoCondominio.Ativo)
            {
                Ativo = novoCondominio.Ativo;
            }
            //if (!string.Equals(Rua, novoCondominio.Rua, StringComparison.OrdinalIgnoreCase))
            //{
            //    Rua = novoCondominio.Rua;
            //}
            //if (!string.Equals(Bairro, novoCondominio.Bairro, StringComparison.OrdinalIgnoreCase))
            //{
            //    Bairro = novoCondominio.Bairro;
            //}
            //if (!string.Equals(Cidade, novoCondominio.Cidade, StringComparison.OrdinalIgnoreCase))
            //{
            //    Cidade = novoCondominio.Cidade;
            //}
            //if (!string.Equals(Estado, novoCondominio.Estado, StringComparison.OrdinalIgnoreCase))
            //{
            //    Estado = novoCondominio.Estado;
            //}
            if (!string.Equals(Logo, novoCondominio.Logo, StringComparison.OrdinalIgnoreCase))
            {
                Logo = novoCondominio.Logo;
            }
            if (!string.Equals(Apartamento, novoCondominio.Apartamento, StringComparison.OrdinalIgnoreCase))
            {
                Apartamento = novoCondominio.Apartamento;
            }
        }
    }
}
