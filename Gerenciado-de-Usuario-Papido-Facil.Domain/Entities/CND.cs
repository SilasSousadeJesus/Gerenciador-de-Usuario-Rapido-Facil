using Gerenciado_de_Usuario_Papido_Facil.CrossCutting.Util.Enum;
using System.Text.RegularExpressions;

namespace Gerenciado_de_Usuario_Papido_Facil.Domain.Entities
{
    public class CND
    {
        public CND() { }

        public CND(string texto, Guid idoneidadeId)
        {
            Tipo = ExtrairTipo(texto);
            CnpjCpf = ExtrairCnpjCpf(texto);

            string textoSemCabecalho = LimparCabecalho(texto);

            IdoneidadeId = idoneidadeId;
            DataEmissao = ExtrairDataEmissao(textoSemCabecalho);
            DataValidade = ExtrairDataValidade(textoSemCabecalho);
            CodigoControle = ExtrairCodigoControle(textoSemCabecalho);

            DataEnvio = DateTime.Now;
        }
        public Guid Id { get; set; }
        public DateTime? DataEmissao { get; set; }
        public DateTime? DataValidade { get; set; }
        public DateTime? DataEnvio { get; set; }
        public DateTime? DataAprovacao { get; set; }
        public DateTime? DataRejeicao { get; set; }
        public string CodigoControle { get; set; } = string.Empty;
        public ETipoCertidao Tipo { get; set; }
        public EEspecieCertidao Especie { get; set; } = EEspecieCertidao.CND;
        public Guid IdoneidadeId { get; set; }
        public string CnpjCpf { get; set; } = string.Empty;

        private string LimparCabecalho(string texto)
        {
            // Encontra o início do CNPJ + 24 caracteres e remove tudo antes dele
            int indiceInicioCNPJ = texto.IndexOf("CNPJ:");
            if (indiceInicioCNPJ == -1) throw new ArgumentException("CNPJ não encontrado no texto.");

            return texto.Substring(indiceInicioCNPJ);
        }

        private DateTime ExtrairDataEmissao(string texto)
        {
            const string chave = "Emitida";
            int indiceInicio = texto.IndexOf(chave) + chave.Length;
            int indiceFim = texto.IndexOf("<hora e data de Brasília>.", indiceInicio);

            if (indiceInicio == -1 || indiceFim == -1) throw new ArgumentException("Data de emissão não encontrada no texto.");

 
            string dataHora = texto.Substring(indiceInicio, indiceFim - indiceInicio);
            string hora = Regex.Match(dataHora, "\\b\\d{2}:\\d{2}:\\d{2}\\b").Value;
            string data = Regex.Match(dataHora, "\\b\\d{2}/\\d{2}/\\d{4}\\b").Value;


            return DateTime.Parse($"{data} {hora}");
        }

        private DateTime ExtrairDataValidade(string texto)
        {
            const string chave = "Válida até";
            int indiceInicio = texto.IndexOf(chave) + chave.Length;
            int indiceFim = texto.IndexOf("Código", indiceInicio);

            if (indiceInicio == -1) throw new ArgumentException("Data de validade não encontrada no texto.");

            string dataValidade = texto.Substring(indiceInicio, indiceFim - indiceInicio);
            string data = Regex.Match(dataValidade, "\\b\\d{2}/\\d{2}/\\d{4}\\b").Value;
            return DateTime.Parse(data);
        }

        private string ExtrairCodigoControle(string texto)
        {
            const string chave = "Código de controle da certidão:";
            int indiceInicio = texto.IndexOf(chave) + chave.Length;
            int indiceFim = texto.IndexOf("Qualquer", indiceInicio);

            if (indiceInicio == -1) throw new ArgumentException("Código de controle não encontrado no texto.");

            return  texto.Substring(indiceInicio, indiceFim - indiceInicio).Replace(" ", "");
        }

        private ETipoCertidao ExtrairTipo(string texto)
        {
            const string chave = "MINISTÉRIO";
            int indiceInicio = texto.IndexOf(chave) + chave.Length;
            int indiceFim = texto.IndexOf("CNPJ", indiceInicio);

            if (indiceInicio == -1 || indiceFim == -1)
                throw new ArgumentException("Cabeçalho não localizado");

            string regiaoDoCabecalho = texto.Substring(indiceInicio, indiceFim - indiceInicio).ToUpperInvariant();

            // Verificar o tipo de certidão com base em palavras-chave no cabeçalho
            if (regiaoDoCabecalho.Contains("CERTIDÃO NEGATIVA DE DÉBITOS")) {
                DataRejeicao = DateTime.Now;
                return ETipoCertidao.Negativa;
            }

            if (regiaoDoCabecalho.Contains("POSITIVA COM EFEITOS DE NEGATIVA DE DÉBITOS")) {
                DataRejeicao = DateTime.Now;
                return ETipoCertidao.PositivaComfeitoNegativa;
            }

            if (regiaoDoCabecalho.Contains("CERTIDÃO POSITIVA DE DÉBITOS")) {
                DataAprovacao = DateTime.Now;
                return ETipoCertidao.Positiva;
            }

            DataRejeicao = DateTime.Now;
            return ETipoCertidao.NaoIdentificado;
        }

        private string ExtrairCnpjCpf(string texto)
        {
            const string chave = "CNPJ:";
            int indiceInicioCNPJ = texto.IndexOf(chave);

            if (indiceInicioCNPJ == -1)
                throw new ArgumentException("CNPJ não encontrado no texto.");

            // Pega o texto a partir de "CNPJ:"
            string textoApartirDoCnpj = texto.Substring(indiceInicioCNPJ + chave.Length).Trim();

            // Extrai apenas os números (remove pontos, traços, barras e espaços)
            string apenasNumeros = Regex.Replace(textoApartirDoCnpj, "[^0-9]", "");

            // Pega apenas os primeiros 14 dígitos (tamanho padrão de CNPJ)
            // ou 11 dígitos se for CPF
            if (apenasNumeros.Length >= 14)
                return apenasNumeros.Substring(0, 14); // CNPJ
            else if (apenasNumeros.Length >= 11)
                return apenasNumeros.Substring(0, 11); // CPF
            else
                throw new ArgumentException("CNPJ/CPF inválido - quantidade de dígitos insuficiente.");
        }
    }
}
