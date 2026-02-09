using Gerenciado_de_Usuario_Papido_Facil.CrossCutting.Util.Enum;
using System.Text.RegularExpressions;

namespace Gerenciado_de_Usuario_Papido_Facil.Domain.Entities
{
    public class CNDT
    {
        public CNDT() { }
        public CNDT(string texto, Guid idoneidadeId)
        {

            Tipo = ExtrairTipo(texto);

            string somenteCabecalho = pegarCabecalho(texto);

            IdoneidadeId = idoneidadeId;
            NumeroCertidao = ExtrairNumeroCertidao(somenteCabecalho);
            DataValidade = ExtrairDataValidade(somenteCabecalho);
            DataExpedicao = ExtrairDataExpedicao(somenteCabecalho);
            CnpjCpf = ExtrairCnpjCpf(somenteCabecalho);

            DataEnvio = DateTime.Now;
        }

        public Guid Id { get; set; }
        public DateTime? DataExpedicao { get; set; }
        public DateTime? DataValidade { get; set; }
        public string NumeroCertidao { get; set; } = string.Empty;
        public string CnpjCpf { get; set; } = string.Empty;
        public DateTime? DataEnvio { get; set; }
        public DateTime? DataAprovacao { get; set; }
        public DateTime? DataRejeicao { get; set; }
        public ETipoCertidao Tipo { get; set; }
        public EEspecieCertidao Especie { get; set; } = EEspecieCertidao.CNDT;
        public Guid IdoneidadeId { get; set; }

         
        private string pegarCabecalho(string texto)
        {
            const string chave = "CERTIDÃO";
            int indiceInicio = texto.IndexOf(chave);
            int indiceFim = texto.IndexOf("Certifica-se", indiceInicio);

            if (indiceInicio == -1 || indiceFim == -1)
                throw new ArgumentException("Cabeçalho não localizado");

            string regiaoDoCabecalho = texto.Substring(indiceInicio, indiceFim - indiceInicio);

            return regiaoDoCabecalho;
        }

        private string ExtrairNumeroCertidao(string somenteCabecalho)
        {
            const string chave = "Certidão nº:";
            int indiceInicio = somenteCabecalho.IndexOf(chave) + chave.Length;
            int indiceFim = somenteCabecalho.IndexOf("Expedição:", indiceInicio);

            if (indiceInicio == -1) throw new ArgumentException("Código de controle não encontrado no texto.");

            return somenteCabecalho.Substring(indiceInicio, indiceFim - indiceInicio).Replace(" ", "");
        }

        private DateTime ExtrairDataValidade(string texto)
        {
            const string chave = "Validade:";
            int indiceInicio = texto.IndexOf(chave) + chave.Length;
            int indiceFim = texto.IndexOf("expedição.", indiceInicio);

            if (indiceInicio == -1 || indiceFim == -1) throw new ArgumentException("Data de emissão não encontrada no texto.");


            string regiaoData = texto.Substring(indiceInicio, indiceFim - indiceInicio);
            string data = Regex.Match(regiaoData, "\\b\\d{2}/\\d{2}/\\d{4}\\b").Value;


            return DateTime.Parse($"{data}");
        }

        private DateTime ExtrairDataExpedicao(string texto)
        {
            const string chave = "Expedição:";
            int indiceInicio = texto.IndexOf(chave) + chave.Length;
            int indiceFim = texto.IndexOf("Validade:", indiceInicio);

            if (indiceInicio == -1 || indiceFim == -1) throw new ArgumentException("Data de emissão não encontrada no texto.");


            string dataHora = texto.Substring(indiceInicio, indiceFim - indiceInicio);
            string hora = Regex.Match(dataHora, "\\b\\d{2}:\\d{2}:\\d{2}\\b").Value;
            string data = Regex.Match(dataHora, "\\b\\d{2}/\\d{2}/\\d{4}\\b").Value;


            return DateTime.Parse($"{data} {hora}");
        }

        private ETipoCertidao ExtrairTipo(string texto)
        {
            const string chave = "CERTIDÃO";
            int indiceInicio = texto.IndexOf(chave);
            int indiceFim = texto.IndexOf("expedição.", indiceInicio);

            if (indiceInicio == -1 || indiceFim == -1)
                throw new ArgumentException("Cabeçalho não localizado");

            string regiaoDoCabecalho = texto.Substring(indiceInicio, indiceFim - indiceInicio).ToUpperInvariant();

            if (regiaoDoCabecalho.Contains("CERTIDÃO NEGATIVA DE DÉBITOS")) {
                DataRejeicao = DateTime.Now;
                return ETipoCertidao.Negativa;
            }

            if (regiaoDoCabecalho.Contains("POSITIVA COM EFEITOS DE NEGATIVA DE DÉBITOS")) {
                DataRejeicao = DateTime.Now;
                return ETipoCertidao.PositivaComfeitoNegativa;
            }

            if (regiaoDoCabecalho.Contains("CERTIDÃO POSITIVA DE DÉBITOS TRABALHISTAS")) {
                DataAprovacao = DateTime.Now;
                return ETipoCertidao.Positiva;
            }
            DataRejeicao = DateTime.Now;

            return ETipoCertidao.NaoIdentificado;
        }
        private string ExtrairCnpjCpf(string somenteCabecalho)
        {
            const string chave = "CNPJ:";
            int indiceInicio = somenteCabecalho.IndexOf(chave) + chave.Length;
            int indiceFim = somenteCabecalho.IndexOf("Certidão nº:", indiceInicio);

            if (indiceInicio == -1 || indiceFim == -1)
                throw new ArgumentException("CNPJ/CPF não encontrado no texto.");

            string cnpjComFormatacao = somenteCabecalho.Substring(indiceInicio, indiceFim - indiceInicio).Trim();

            return Regex.Replace(cnpjComFormatacao, "[^0-9]", "");
        }

    }
}
