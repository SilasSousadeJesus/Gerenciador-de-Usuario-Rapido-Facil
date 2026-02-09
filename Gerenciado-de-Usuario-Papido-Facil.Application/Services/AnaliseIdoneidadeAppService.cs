using Gerenciado_de_Usuario_Papido_Facil.Application.Interfaces;
using Gerenciado_de_Usuario_Papido_Facil.Application.ViewModel;
using Gerenciado_de_Usuario_Papido_Facil.CrossCutting.Extensions;
using Gerenciado_de_Usuario_Papido_Facil.CrossCutting.Util.Enum;
using Gerenciado_de_Usuario_Papido_Facil.Domain.Entities;
using Gerenciado_de_Usuario_Papido_Facil.Infra.Data.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Text;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;


namespace Gerenciado_de_Usuario_Papido_Facil.Application.Services
{
    public class AnaliseIdoneidadeAppService : IAnaliseIdoneidadeAppService
    {
        private readonly IHttpAppService _httpAppService;
        private readonly IConfiguration _configuration;
        private readonly IAnaliseIdoneidadeRepository _analiseIdoneidadeRepository;
        private readonly IEmpresaPrestadoraRepository _empresaPrestadoraRepository;

        public AnaliseIdoneidadeAppService(
            IHttpAppService httpAppService, 
            IConfiguration configuration,
            IAnaliseIdoneidadeRepository analiseIdoneidadeRepository,
            IEmpresaPrestadoraRepository empresaPrestadoraRepository
            )
        {
            _httpAppService = httpAppService;
            _configuration = configuration; 
            _analiseIdoneidadeRepository = analiseIdoneidadeRepository;
            _empresaPrestadoraRepository = empresaPrestadoraRepository;
        }

        public async Task<RetornoGenerico> CriarIdoneidade(Guid empresaId)
        {
            try
            {
                var idoneidade = new Idoneidade(empresaId);

                var respostaAnaliseIdoneidade = await _analiseIdoneidadeRepository.CadastrarIdoneidadeAsync(idoneidade);

                if (respostaAnaliseIdoneidade is null)
                {

                    return new RetornoGenerico(false,
                            "Erro ao criar a idoneidade do prestador",
                            "Erro ao criar a idoneidade do prestador",
                            HttpStatusCode.InternalServerError
                        );
                }

                return new RetornoGenerico(true,
                        "Idoneidade criada com sucesso",
                        "Idoneidade criada com sucesso",
                        HttpStatusCode.Created
                    );
            }
            catch (Exception ex)
            {
                    return new RetornoGenerico(false,
                                      ex.Message,
                                      ex.InnerException.ToString(),
                                      HttpStatusCode.InternalServerError
                                  );
            }
        }

        public async Task<RetornoGenerico> LerCertidoes(Guid empresaId, string pdfBase64, EEspecieCertidao eEspecieCertidao) {

            var empresa = await _empresaPrestadoraRepository.BuscarEmpresa(empresaId);
            var idoneidade = await _analiseIdoneidadeRepository.BuscarIdoneidadeAsync(empresaId);

            if(empresa is null || idoneidade is null) return new RetornoGenerico(false,
                                 "Idoneidade ou empresa não encontrada",
                                 "Idoneidade ou empresa não encontrada",
                                 HttpStatusCode.NotFound
                            );

            if (string.IsNullOrEmpty(pdfBase64)) return new RetornoGenerico(false,
                     "pdfBase64 não fornecido",
                     "pdfBase64 não fornecido",
                     HttpStatusCode.BadRequest
                );

            if (eEspecieCertidao == EEspecieCertidao.CND) await LerCND(idoneidade, empresa, pdfBase64);
            if (eEspecieCertidao == EEspecieCertidao.CNDT) await LerCNDT(idoneidade, empresa, pdfBase64);
            if (eEspecieCertidao == EEspecieCertidao.NaoIdentificado) return new RetornoGenerico(false,
                                                                     "Certidão não identificada",
                                                                     "Certidão não identificada",
                                                                     HttpStatusCode.BadRequest); 

            var idoneidadeAtualizada = await _analiseIdoneidadeRepository.BuscarIdoneidadeAsync(empresa.Id);

            idoneidade.VerificarIdoneidade();

            await _analiseIdoneidadeRepository.EditarIdoneidadeAsync(idoneidade);

            return new RetornoGenerico(
                                        true,
                                        "analise feita com sucesso",
                                        "analise feita com sucesso",
                                        HttpStatusCode.OK,
                                        idoneidade
                                         );

        }

        private async Task<RetornoGenerico> LerCND(Idoneidade idoneidade, EmpresaPrestadoraViewModel empresa, string pdfBase64)
        {
            string documentosPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string pastaCertidoes = Path.Combine(documentosPath, "RapidoFacilDocumentos/CND");

            try
            {
                // Cria a pasta de certidões, se não existir
                if (!Directory.Exists(pastaCertidoes))
                {
                    Directory.CreateDirectory(pastaCertidoes);
                }

                // Converte o base64 para bytes
                byte[] pdfBytes = Convert.FromBase64String(pdfBase64);

                // Valida se é realmente um arquivo PDF
                if (!ValidarPdf(pdfBytes))
                {
                    return new RetornoGenerico(
                        false,
                        "Arquivo inválido",
                        "O arquivo enviado não é um PDF válido",
                        HttpStatusCode.BadRequest
                    );
                }

                // Gera um nome único para o arquivo PDF
                string nomeArquivoPdf = $"{empresa.CnpjCpf}-CND-{DateTime.Now:yyyyMMdd-HHmmss}.pdf";
                string caminhoArquivoPdf = Path.Combine(pastaCertidoes, nomeArquivoPdf);

                // Salva o arquivo PDF na pasta
                await File.WriteAllBytesAsync(caminhoArquivoPdf, pdfBytes);

                Console.WriteLine($"PDF salvo em: {caminhoArquivoPdf}");

                // Define a pasta onde os arquivos .txt serão salvos
                string pastaSaida = Path.Combine(documentosPath, "RapidoFacilDocumentos/CertidoesTexto");

                // Cria a pasta de saída, se não existir
                if (!Directory.Exists(pastaSaida))
                {
                    Directory.CreateDirectory(pastaSaida);
                }

                // Agora processa o arquivo PDF que acabou de ser salvo
                string[] arquivosPdf = Directory.GetFiles(pastaCertidoes, "*.pdf");

                if (arquivosPdf.Length == 0)
                {
                    throw new ArgumentException("Nenhum arquivo PDF encontrado na pasta.");
                }

                // Seleciona o PDF mais recente (que será o que acabamos de salvar)
                string arquivoPdf = arquivosPdf
                    .OrderByDescending(File.GetLastWriteTime)
                    .FirstOrDefault();

                if (arquivoPdf == null)
                {
                    throw new ArgumentException("Nenhum arquivo PDF recente encontrado na pasta.");
                }

                Console.WriteLine($"Processando arquivo: {Path.GetFileName(arquivoPdf)}");

                StringBuilder textoExtraido = new StringBuilder();

                using (PdfDocument pdf = PdfDocument.Open(arquivoPdf))
                {
                    foreach (var pagina in pdf.GetPages())
                    {
                        string textoPagina = ExtrairTextoDaPagina(pagina);
                        textoExtraido.AppendLine(textoPagina);
                    }
                }

                var certidao = new CND(textoExtraido.ToString(), idoneidade.Id);

                //if (certidao.CnpjCpf != empresa.CnpjCpf) return new RetornoGenerico(
                //        false,
                //        "Esta certidão não pertence a empresa em questão",
                //        "Esta certidão não pertence a empresa em questão",
                //        HttpStatusCode.BadRequest
                //    );

                if (idoneidade.CND.Any())
                {
                    var jaExisteCertidao = idoneidade.CND.Where(x => x.CodigoControle == certidao.CodigoControle).FirstOrDefault();

                    if (jaExisteCertidao != null)
                        return new RetornoGenerico(
                            true,
                            "Certidao já consta na base",
                            "Certidao já consta na base",
                            HttpStatusCode.BadRequest,
                            idoneidade
                        );
                }

                await _analiseIdoneidadeRepository.CadastrarCND(certidao);

                string nomeArquivoTxt = $"{empresa.CnpjCpf}-CND-{Guid.NewGuid()}.txt";
                string caminhoArquivoTxt = Path.Combine(pastaSaida, nomeArquivoTxt);
                File.WriteAllText(caminhoArquivoTxt, textoExtraido.ToString());

                return new RetornoGenerico(
                    true,
                    "analise feita com sucesso",
                    "analise feita com sucesso",
                    HttpStatusCode.OK,
                    idoneidade
                );
            }
            catch (FormatException ex)
            {
                return new RetornoGenerico(
                    false,
                    "Formato de base64 inválido",
                    ex.Message,
                    HttpStatusCode.BadRequest
                );
            }
            catch (Exception ex)
            {
                return new RetornoGenerico(
                    false,
                    ex.Message,
                    ex.InnerException?.ToString() ?? ex.Message,
                    HttpStatusCode.InternalServerError
                );
            }
        }

        private async Task<RetornoGenerico> LerCNDT(Idoneidade idoneidade, EmpresaPrestadoraViewModel empresa, string pdfBase64)
        {
            string documentosPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string pastaCertidoes = Path.Combine(documentosPath, "RapidoFacilDocumentos/CNDT");
            string pastaSaida = Path.Combine(documentosPath, "RapidoFacilDocumentos/CertidoesTexto");

            try
            {
                // Cria a pasta de certidões, se não existir
                if (!Directory.Exists(pastaCertidoes))
                {
                    Directory.CreateDirectory(pastaCertidoes);
                }

                // Converte o base64 para bytes
                byte[] pdfBytes = Convert.FromBase64String(pdfBase64);


                // Valida se é realmente um arquivo PDF
                if (!ValidarPdf(pdfBytes))
                {
                    return new RetornoGenerico(
                        false,
                        "Arquivo inválido",
                        "O arquivo enviado não é um PDF válido",
                        HttpStatusCode.BadRequest
                    );
                }

                // Gera um nome único para o arquivo PDF
                string nomeArquivoPdf = $"{empresa.CnpjCpf}-CNDT-{DateTime.Now:yyyyMMdd-HHmmss}.pdf";
                string caminhoArquivoPdf = Path.Combine(pastaCertidoes, nomeArquivoPdf);

                // Salva o arquivo PDF na pasta
                await File.WriteAllBytesAsync(caminhoArquivoPdf, pdfBytes);

                Console.WriteLine($"PDF salvo em: {caminhoArquivoPdf}");

                // Define a pasta onde os arquivos .txt serão salvos
                pastaSaida = Path.Combine(documentosPath, "RapidoFacilDocumentos/CertidoesTexto");

                // Cria a pasta de saída, se não existir
                if (!Directory.Exists(pastaSaida))
                {
                    Directory.CreateDirectory(pastaSaida);
                }

                // Agora processa o arquivo PDF que acabou de ser salvo
                string[] arquivosPdf = Directory.GetFiles(pastaCertidoes, "*.pdf");

                if (arquivosPdf.Length == 0)
                {
                    throw new ArgumentException("Nenhum arquivo PDF encontrado na pasta.");
                }

                // Seleciona o PDF mais recente (que será o que acabamos de salvar)
                string arquivoPdf = arquivosPdf
                    .OrderByDescending(File.GetLastWriteTime)
                    .FirstOrDefault();

                if (arquivoPdf == null)
                {
                    throw new ArgumentException("Nenhum arquivo PDF recente encontrado na pasta.");
                }

                Console.WriteLine($"Processando arquivo: {Path.GetFileName(arquivoPdf)}");

                StringBuilder textoExtraido = new StringBuilder();

                using (PdfDocument pdf = PdfDocument.Open(arquivoPdf))
                {
                    foreach (var pagina in pdf.GetPages())
                    {
                        string textoPagina = ExtrairTextoDaPagina(pagina);
                        textoExtraido.AppendLine(textoPagina);
                    }
                }

                var certidao = new CNDT(textoExtraido.ToString(), idoneidade.Id);

                //if(certidao.CnpjCpf != empresa.CnpjCpf) return new RetornoGenerico(
                //    false,
                //    "Esta certidão não pertence a empresa em questão",
                //    "Esta certidão não pertence a empresa em questão",
                //    HttpStatusCode.BadRequest
                //);

                if (idoneidade.CNDT.Any())
                {
                    var jaExisteCertidao = idoneidade.CNDT.Where(x => x.NumeroCertidao == certidao.NumeroCertidao).FirstOrDefault();

                    if (jaExisteCertidao != null)
                        return new RetornoGenerico(
                            true,
                            "Certidao já consta na base",
                            "Certidao já consta na base",
                            HttpStatusCode.BadRequest,
                            idoneidade
                        );
                }

                await _analiseIdoneidadeRepository.CadastrarCNDT(certidao);

                string nomeArquivoTxt = $"{empresa.CnpjCpf}-CNDT-{Guid.NewGuid()}.txt";
                string caminhoArquivoTxt = Path.Combine(pastaSaida, nomeArquivoTxt);
                File.WriteAllText(caminhoArquivoTxt, textoExtraido.ToString());

                return new RetornoGenerico(
                    true,
                    "analise feita com sucesso",
                    "analise feita com sucesso",
                    HttpStatusCode.OK,
                    idoneidade
                );
            }
            catch (FormatException ex)
            {
                return new RetornoGenerico(
                    false,
                    "Formato de base64 inválido",
                    ex.Message,
                    HttpStatusCode.BadRequest
                );
            }
            catch (Exception ex)
            {
                return new RetornoGenerico(
                    false,
                    ex.Message,
                    ex.InnerException?.ToString() ?? ex.Message,
                    HttpStatusCode.InternalServerError
                );
            }
        }

        private string ExtrairTextoDaPagina(Page pagina)
        {
            StringBuilder texto = new StringBuilder();

            // Usando uma lista para armazenar as palavras
            List<Word> palavras = pagina.GetWords().ToList();

            if (palavras.Any())
            {
                // Adiciona a primeira palavra sem espaço
                texto.Append(palavras[0].Text);

                // Itera sobre as palavras para inserir espaços entre elas
                for (int i = 1; i < palavras.Count; i++)
                {
                    var palavraAtual = palavras[i];
                    var palavraAnterior = palavras[i - 1];

                    // Verifica se há um espaço suficiente entre as palavras
                    if (Math.Abs(palavraAtual.BoundingBox.Left - palavraAnterior.BoundingBox.Right) > 0.5)
                    {
                        texto.Append(" ");
                    }

                    texto.Append(palavraAtual.Text);
                }
            }

            return texto.ToString();
        }

        private void DeletarArquivosComUmAno(string pastaSubPasta) {
            try
            {
                // Caminho da pasta
                string documentosPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string pastaCertidoes = Path.Combine(documentosPath, pastaSubPasta);

                // Verifique se a pasta existe
                if (Directory.Exists(pastaCertidoes))
                {
                    // Obtenha todos os arquivos PDF na pasta
                    string[] arquivos = Directory.GetFiles(pastaCertidoes, "*.pdf");

                    // Data limite: um ano atrás
                    DateTime dataLimite = DateTime.Now.AddYears(-1);

                    foreach (string arquivo in arquivos)
                    {
                        // Obtenha a data de criação do arquivo
                        DateTime dataCriacao = File.GetCreationTime(arquivo);

                        // Verifique se o arquivo é mais antigo que a data limite
                        if (dataCriacao < dataLimite)
                        {
                            // Exclua o arquivo
                            File.Delete(arquivo);
                            Console.WriteLine($"Arquivo deletado: {arquivo}");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("A pasta não existe.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao deletar arquivos: {ex.Message}");
            }
        }

        public async Task<RetornoGenerico> BuscarIdoneidade(Guid empresaId)
        {
            var empresa = await _empresaPrestadoraRepository.BuscarEmpresa(empresaId);
            var idoneidade = await _analiseIdoneidadeRepository.BuscarIdoneidadeAsync(empresaId);

            if (empresa is null || idoneidade is null) return new RetornoGenerico(false,
                                 "Idoneidade ou empresa não encontrada",
                                 "Idoneidade ou empresa não encontrada",
                                 HttpStatusCode.NotFound
                            );

            var idoneidadeAtualizada = await _analiseIdoneidadeRepository.BuscarIdoneidadeAsync(empresa.Id);


            IdoneidadeViewModel viewModel = new IdoneidadeViewModel(idoneidade);

            return new RetornoGenerico( true,
                                        "analise feita com sucesso",
                                        "analise feita com sucesso",
                                        HttpStatusCode.OK,
                                        viewModel
                                        );

        }

        private bool ValidarPdf(byte[] fileBytes)
        {
            if (fileBytes == null || fileBytes.Length < 5)
                return false;

            // PDF sempre começa com %PDF- (25 50 44 46 2D em hexadecimal)
            return fileBytes[0] == 0x25 && // %
                   fileBytes[1] == 0x50 && // P
                   fileBytes[2] == 0x44 && // D
                   fileBytes[3] == 0x46 && // F
                   fileBytes[4] == 0x2D;   // -
        }
            

    
    
    }
}
