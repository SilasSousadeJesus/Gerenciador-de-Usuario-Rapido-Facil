using Gerenciado_de_Usuario_Rapido_Facil.CrossCutting.Util.Enum;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using System.Text;

namespace Gerenciado_de_Usuario_Rapido_Facil.Domain.Entities
{
    public class Condominio
    {
        public Condominio() { }
        public Condominio(
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
        }

        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string CnpjCpf { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public bool Ativo { get; set; } = true;
        public bool PeriodoTeste { get; set; } = true;
        public string CodigoVinculacao { get; set; } = string.Empty;
        public EPorteCondominio PorteCondominio { get; set; }

        public string Rua { get; set; } = string.Empty;
        public string Bairro { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string Logo { get; set; } = string.Empty;
        public string Cep { get; set; } = string.Empty;
        public DateTime DataCadastro { get; set; } = DateTime.Now;
        public bool AceitouTermosDeUso { get; set; } = false;

        public List<Condomino>? Condominos { get; set; }

        public void GerarCodigoVinculacao()
        {
            var nomeLimpo = LimparNome(this.Nome, 5); // 5 caracteres do nome
            var hashUnico = GerarHashUnico(this.Nome, this.CnpjCpf, 4); // Hash do NOME + CNPJ

            // @NOME-HASH = @5chars-4chars = 10 caracteres
            this.CodigoVinculacao = $"@{nomeLimpo}{hashUnico}".ToUpper();
        }

        private string LimparNome(string nome, int maxCaracteres)
        {
            // Remove "Condomínio", "Condominio", "Cond.", "Cond" do início (case insensitive)
            var nomeProcessado = nome;
            var prefixos = new[] { "condomínio", "condominio", "cond.", "cond" };

            foreach (var prefixo in prefixos)
            {
                if (nomeProcessado.StartsWith(prefixo, StringComparison.OrdinalIgnoreCase))
                {
                    nomeProcessado = nomeProcessado.Substring(prefixo.Length).TrimStart();
                    break;
                }
            }

            // Remove espaços e caracteres especiais, mantém apenas letras e números
            var nomeSemEspacos = new string(nomeProcessado
                .Where(c => char.IsLetterOrDigit(c))
                .ToArray());

            return nomeSemEspacos.Length > maxCaracteres
                ? nomeSemEspacos.Substring(0, maxCaracteres)
                : nomeSemEspacos.PadRight(maxCaracteres, '0');
        }

        private string GerarHashUnico(string nome, string cnpj, int tamanho)
        {
            // Combina NOME COMPLETO + CNPJ para garantir unicidade
            var textoUnico = $"{nome}{cnpj}";

            using (var md5 = MD5.Create())
            {
                var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(textoUnico));
                var base64 = Convert.ToBase64String(hash)
                    .Replace("+", "")  // Remove caracteres especiais
                    .Replace("/", "")  // Remove caracteres especiais
                    .Replace("=", ""); // Remove caracteres especiais

                return base64.Substring(0, tamanho); // Retorna apenas letras e números
            }
        }
        public void AtualizarCondominio(Condominio novoCondominio)
        {
            if (!string.Equals(Cep, novoCondominio.Cep, StringComparison.OrdinalIgnoreCase))
            {
                Cep = novoCondominio.Cep;
            }
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
            if (!string.Equals(Rua, novoCondominio.Rua, StringComparison.OrdinalIgnoreCase))
            {
                Rua = novoCondominio.Rua;
            }
            if (!string.Equals(Bairro, novoCondominio.Bairro, StringComparison.OrdinalIgnoreCase))
            {
                Bairro = novoCondominio.Bairro;
            }
            if (!string.Equals(Cidade, novoCondominio.Cidade, StringComparison.OrdinalIgnoreCase))
            {
                Cidade = novoCondominio.Cidade;
            }
            if (!string.Equals(Estado, novoCondominio.Estado, StringComparison.OrdinalIgnoreCase))
            {
                Estado = novoCondominio.Estado;
            }
            if (!string.Equals(Logo, novoCondominio.Logo, StringComparison.OrdinalIgnoreCase))
            {
                Logo = novoCondominio.Logo;
            }
            if (PeriodoTeste != novoCondominio.PeriodoTeste)
            {
                PeriodoTeste = novoCondominio.PeriodoTeste;
            }
        }
        public int ContarCondominos()
        {
            if (Condominos is null)
            {
                PorteCondominio = EPorteCondominio.SemCondominos;
                return 0;
            }

            int quantidade = Condominos.Count();

            PorteCondominio = quantidade switch
            {
                <= 50 => EPorteCondominio.Ate50Condominos,
                <= 200 => EPorteCondominio.Ate200Condominos,
                <= 500 => EPorteCondominio.Ate500Condominos,
                _ => EPorteCondominio.Ate1000Condominos
            };

            return quantidade;
        }

    }
}