using AutoMapper;
using Gerenciado_de_Usuario_Rapido_Facil.Application.DTOs;
using Gerenciado_de_Usuario_Rapido_Facil.Application.Interfaces;
using Gerenciado_de_Usuario_Rapido_Facil.Application.ViewModel;
using Gerenciado_de_Usuario_Rapido_Facil.CrossCutting.Extensions;
using Gerenciado_de_Usuario_Rapido_Facil.CrossCutting.Util.Mocks;
using Gerenciado_de_Usuario_Rapido_Facil.Domain.Entities;
using Gerenciado_de_Usuario_Rapido_Facil.Infra.Data.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Text;

namespace Gerenciado_de_Usuario_Rapido_Facil.Application.Services
{
    public class CondominioAppService : ICondominioAppService
    {
        private readonly IMapper _mapper;
        private readonly ICondominioRepository _condominioRepository;
        private readonly IValidacaoParaCadastroRepository _validacaoParaCadastroUsuario;
        private readonly IConfiguration _configuration;
        private readonly IHttpAppService _httpAppService;
        private readonly IEmailAppService _emailAppService;
        private static readonly Random _random = new();

        private readonly ITemplateHTMLRepository _templateHTMLRepository;
        public CondominioAppService(
                ICondominioRepository condominioRepository, 
                IMapper mapper,
                IValidacaoParaCadastroRepository validacaoParaCadastroRepository,
                IConfiguration configuration,
                IHttpAppService httpAppService,
                IEmailAppService emailAppService,
                 ITemplateHTMLRepository templateHTMLRepository
            )
        {
            _mapper = mapper;
            _condominioRepository = condominioRepository;
            _validacaoParaCadastroUsuario = validacaoParaCadastroRepository;
            _configuration = configuration;
            _httpAppService = httpAppService;
            _emailAppService = emailAppService;
            _templateHTMLRepository = templateHTMLRepository;
        }

        // CRUD
        public async Task<RetornoGenerico> CadastrarCondominioAsync(CadastroCondominioDTO condominioDTO)
        {
            var retorno = new RetornoGenerico();
            try
            {
                var valido = await _validacaoParaCadastroUsuario.VerificarEmailExiste(condominioDTO.Email);

                if (valido) {
                    return new RetornoGenerico(true,
                        "Este Email já esta cadastrado",
                        "Este Email já esta cadastrado",
                        HttpStatusCode.BadRequest                     
                        );
                }

                condominioDTO.Senha =  HashSenha.HashSenhaUsuario(condominioDTO.Senha);

                var condominio = _mapper.Map<Condominio>(condominioDTO);

                condominio.GerarCodigoVinculacao();
                condominio.ContarCondominos();

               var novoCondominioCadastrado = await _condominioRepository.CadastrarCondominioAsync(condominio);

                var ulr = _configuration.GetSection("CotacaoApi")["CriarCondominio"];

                if (string.IsNullOrEmpty(ulr)) return new RetornoGenerico(false,
                                                "URL do gerenciador de usuario não fornecedia",
                                                "URL do gerenciador de usuario não fornecedia",
                                                 HttpStatusCode.BadRequest
                                             );

                if (novoCondominioCadastrado != null) {
                    var response = await _httpAppService.PostAsync(ulr.Replace("{condominioId}", novoCondominioCadastrado.Id.ToString()).Replace("{nome}", novoCondominioCadastrado.Nome), null);

                    if (response is null || !response.IsSuccessStatusCode)
                    {
                        await DeletarCondominioAsync(novoCondominioCadastrado.Id);

                        return new RetornoGenerico(false,
                        "Não foi possivel enviar o id do condominio para a api de cotação",
                        "Não foi possivel enviar o id do condominio para a api de cotação",
                        HttpStatusCode.BadRequest,
                        null
                       );
                    }               
                }

                return new RetornoGenerico(true,
                    "condominio criado",
                    "O cadastro do seu condominio foi feito com sucesso",
                    HttpStatusCode.Created
                );
            }
            catch (Exception ex)
            {
                retorno.Sucesso = false;
                retorno.HttpStatusCode = HttpStatusCode.InternalServerError;
                retorno.MensagemSistema = $"{ex}";
                retorno.MensagemUsuario = "Não foi possivel criar um condominio";
                retorno.Dados = null;
                return retorno;
            }
        }

        public async Task<RetornoGenerico> BuscarTodosOsCondominioAsync(BuscarCondominioPorFiltrosDTO filtros)
        {
            var condominios = await _condominioRepository.BuscarTodosOsCondominiosAsync(
                filtros.nome, 
                filtros.Cnpj,
                filtros.email,
                filtros.CodigoVinculacao, 
                filtros.Cidade, 
                filtros.Estado, 
                filtros.ativo,
                filtros.PeriodoTeste,
                filtros.dataCadastro, 
                filtros.pagina, 
                filtros.itensPorPagina
               );

            var mensagem = condominios.Count > 0 ? $"{condominios.Count} condominios cadastrados" : "Não há condominios cadastrados";
            var sucesso = condominios.Count > 0;

            return new RetornoGenerico
            {
                Sucesso = sucesso,
                HttpStatusCode =  HttpStatusCode.OK,
                MensagemSistema = mensagem,
                MensagemUsuario = mensagem,
                Dados = condominios
            };
        }

        public async Task<RetornoGenerico> BuscarUmCondominioAsync(Guid condominioId)
        {
            var condominio = await _condominioRepository.BuscarUmCondominioAsync(condominioId);

           var mensagemSistema = condominio == null ? "condominio não encontrado" : "condominio encontrado";
           var mensagemUsuario = condominio == null ? "condominio não encontrado" : "condominio encontrado";

            var resultado = condominio == null ? false : true;

            return new RetornoGenerico
            {
                Sucesso = resultado,
                HttpStatusCode = resultado ? System.Net.HttpStatusCode.OK : System.Net.HttpStatusCode.NotFound,
                MensagemSistema = mensagemSistema,
                MensagemUsuario = mensagemUsuario,
                Dados = condominio
            };
        }

        public async Task<RetornoGenerico> BuscarUmCondominioCompletoAsync(Guid condominioId)
        {
            var condominio = await _condominioRepository.BuscarUmCondominioCompletoAsync(condominioId);

            var mensagemSistema = condominio == null ? "condominio não encontrado" : "condominio encontrado";
            var mensagemUsuario = condominio == null ? "condominio não encontrado" : "condominio encontrado";

            var resultado = condominio == null ? false : true;

            return new RetornoGenerico
            {
                Sucesso = resultado,
                HttpStatusCode = resultado ? System.Net.HttpStatusCode.OK : System.Net.HttpStatusCode.NotFound,
                MensagemSistema = mensagemSistema,
                MensagemUsuario = mensagemUsuario,
                Dados = condominio
            };
        }

        public async Task<RetornoGenerico> DeletarCondominioAsync(Guid condominioId)
        {
            var retorno = new RetornoGenerico();

            try
            {
                var buscaPorCondominio = await BuscarUmCondominioCompletoAsync(condominioId);

                if (!buscaPorCondominio.Sucesso)
                {
                    retorno.Sucesso = buscaPorCondominio.Sucesso;
                    retorno.HttpStatusCode = buscaPorCondominio.HttpStatusCode;
                    retorno.MensagemSistema = buscaPorCondominio.MensagemSistema;
                    retorno.MensagemUsuario = buscaPorCondominio.MensagemUsuario;
                    retorno.Dados = null;

                    return retorno;
                }

                var condominio = await _condominioRepository.DeletarCondominioAsync(buscaPorCondominio.Dados);

                retorno.Sucesso = true;
                retorno.HttpStatusCode = HttpStatusCode.OK;
                retorno.MensagemSistema = "Condominio deletado com sucesso";
                retorno.MensagemUsuario =  "Condominio deletado com sucesso";
                retorno.Dados = null;

                return retorno;

            }
            catch (Exception ex)
            {
                retorno.Sucesso = false;
                retorno.HttpStatusCode = HttpStatusCode.InternalServerError;
                retorno.MensagemSistema = $"{ex}";
                retorno.MensagemUsuario = "Não foi possivel Deletar a conta";
                retorno.Dados = null;
                return retorno;
            }
        }

        public async Task<RetornoGenerico> EditarCondominioAsync(Guid condominioId, EdicaoCondominioDTO edicaoCondominioDTO)
        {
            var buscaCondominio = await BuscarUmCondominioCompletoAsync(condominioId);

            if (!buscaCondominio.Sucesso)
            {
                return buscaCondominio;
            }

            Condominio condominio = buscaCondominio.Dados;

            var dadosAtualizados = new Condominio(
                edicaoCondominioDTO.Nome ?? condominio.Nome,
                condominio.Senha,
                edicaoCondominioDTO.CnpjCpf ?? condominio.CnpjCpf,
                edicaoCondominioDTO.Email ?? condominio.Email,
                edicaoCondominioDTO.Logo ?? condominio.Logo,
                edicaoCondominioDTO.Ativo ?? condominio.Ativo,
                edicaoCondominioDTO.Rua ?? condominio.Rua,
                edicaoCondominioDTO.Bairro ?? condominio.Bairro,
                edicaoCondominioDTO.Cidade ?? condominio.Cidade,
                edicaoCondominioDTO.Estado ?? condominio.Estado,
                edicaoCondominioDTO.Cep ?? condominio.Cep
            );


            condominio.AtualizarCondominio(dadosAtualizados);

            condominio.GerarCodigoVinculacao();

            await _condominioRepository.EditarCondominioAsync(condominio);

            return new RetornoGenerico
            {
                Sucesso = true,
                HttpStatusCode = HttpStatusCode.OK,
                MensagemSistema = "condominio atualizado com sucesso",
                MensagemUsuario = "condominio atualizado com sucesso",
                Dados = null
            };
        }

        public async Task<RetornoGenerico> BuscarUmCondominioPorCodigoVinculacaoAsync(string codigovalidacao)
        {
            var condominio = await _condominioRepository.BuscarPorCodigoVinculacaoAsync(codigovalidacao);

            var mensagemSistema = condominio == null ? "condominio não encontrado" : "condominio Encontrado";
            var mensagemUsuario = condominio == null ? "condominio não encontrado" : "condominio Encontrado";

            var resultado = condominio == null ? false : true;

            if (resultado) {
                condominio.Senha = string.Empty;
            }

            return new RetornoGenerico
            {
                Sucesso = resultado,
                HttpStatusCode = resultado ? System.Net.HttpStatusCode.OK : System.Net.HttpStatusCode.NotFound,
                MensagemSistema = mensagemSistema,
                MensagemUsuario = mensagemUsuario,
                Dados = condominio
            };
        }

        public async Task<CondominioViewModel?> BuscarUmCondominioPorEmailAsync(string email)
        {
           var condominio = await _condominioRepository.BuscarCondomonioPorEmail(email);

            if (condominio is null) return null;

            var condominioViewModel = _mapper.Map<CondominioViewModel>(condominio);

            return condominioViewModel;
        }

        public async Task<RetornoGenerico> FinalizarPeriodoTeste(Guid condominioId)
        {
            var condominio = await _condominioRepository.BuscarUmCondominioCompletoAsync(condominioId);

            if (condominio == null)
            {
                return new RetornoGenerico
                {
                    Sucesso = false,
                    HttpStatusCode = HttpStatusCode.NotFound,
                    MensagemSistema = "condominio não encontrado",
                    MensagemUsuario = "condominio não encontrado",
                    Dados = null
                };
            }

            condominio.PeriodoTeste = false;

            condominio.AtualizarCondominio(condominio);

            await _condominioRepository.EditarCondominioAsync(condominio);

            return new RetornoGenerico
            {
                Sucesso = true,
                HttpStatusCode = HttpStatusCode.OK,
                MensagemSistema = "condominio atualizado com sucesso",
                MensagemUsuario = "condominio atualizado com sucesso",
                Dados = null
            };
        }

        public async Task<int> ContarCondominos(Guid condominioId)
        {
            var condominos = await _condominioRepository.BuscarCondominosDeCondominioAsync(condominioId);
            return condominos.Count();  
        }

        public async Task<RetornoGenerico> EnviarEmailVincularCondomino(string destinatario, Guid usuarioIdRemetente)
        {
            var retorno = new RetornoGenerico();
            try
            {
                //var emailDestinatario = await _validacaoParaCadastroUsuario.VerificarEmailExiste(destinatario);

                //if (!emailDestinatario)
                //{
                //    return new RetornoGenerico(true,
                //        "Este Email do destinatario não encontrado",
                //        "Este Email do destinatario não encontrado",
                //        HttpStatusCode.NotFound
                //        );
                //}

                var condominioResultado = await BuscarUmCondominioAsync(usuarioIdRemetente);

                if (!condominioResultado.Sucesso)
                {
                    return new RetornoGenerico(true,
                        "condominio não encontrado",
                        "condominio não encontrado",
                        HttpStatusCode.NotFound
                        );
                }

                Condominio condominio = condominioResultado.Dados;

                var templatesHtml = _configuration.GetSection("TemplatesHtml");
                var templateId = Guid.Parse(templatesHtml["VinculoCondominioCondomino"]);

                var templateHtml = await _templateHTMLRepository.BuscarTemplateHTMLAsync(templateId);

                var body = templateHtml.Corpo.Replace("[code]", condominio.CodigoVinculacao);

                 await _emailAppService.EnviarEmailVinculo(destinatario, "testando", body);

                return new RetornoGenerico(true,
                    "Email enviado com sucesso",
                    "Email enviado com sucesso",
                    HttpStatusCode.OK
                );
            }
            catch (Exception ex)
            {
                retorno.Sucesso = false;
                retorno.HttpStatusCode = HttpStatusCode.InternalServerError;
                retorno.MensagemSistema = $"{ex}";
                retorno.MensagemUsuario = "Não foi possivel enviar o email";
                retorno.Dados = null;
                return retorno;
            }
        }

        public async Task<RetornoGenerico> TrocarSenha(Guid condominioId, TrocaSenhaDTO trocaSenhaDTO)
        {
            var ulr = _configuration.GetSection("AutenticacaoApi")["autenticacaoCondominio"];

            if (string.IsNullOrEmpty(ulr)) return new RetornoGenerico(false,
                                                "URL para autenticação  de usuario não fornecida",
                                                "URL para autenticação  de usuario não fornecida",
                                                HttpStatusCode.BadRequest
                                            );
            var buscaCondominio = await BuscarUmCondominioCompletoAsync(condominioId);

            if (!buscaCondominio.Sucesso) {
                return new RetornoGenerico(false,
                      "Condominio não encontrado",
                      "Condominio não encontrado",
                      HttpStatusCode.NotFound,
                      null
                 );
            }

            Condominio condominio = buscaCondominio.Dados;

            var dto = new LoginDTO() {
                Email = buscaCondominio.Dados.Email,
                Senha = trocaSenhaDTO.SenhaAntiga,
            };

            var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");

            var response = await _httpAppService.PostAsync(ulr, content);

            if (response is null || !response.IsSuccessStatusCode)
            {
                return new RetornoGenerico(false,
                    "Credencias invalidas",
                    "Credencias invalidas",
                    HttpStatusCode.Unauthorized,
                    null
               );
            }

            condominio.Senha = HashSenha.HashSenhaUsuario(trocaSenhaDTO.SenhaNova);

            await  _condominioRepository.EditarCondominioAsync(condominio);

            return new RetornoGenerico(true,
                    "Senha atualizada com sucesso",
                    "Senha atualizada com sucesso",
                    HttpStatusCode.OK,
                    null
                );
        }

        public async Task<RetornoGenerico> CadastrarCondominiosParaTesteAsync(int quantidade)
        {
            var retorno = new RetornoGenerico();

            try
            {
                var condominios = new List<Condominio>();

                for (int i = 1; i <= quantidade; i++)
                {
                    var numero = i.ToString("D2");
                    var email = $"condominio{numero}@gmail.com";

                    var emailExiste = await _validacaoParaCadastroUsuario.VerificarEmailExiste(email);

                    if (emailExiste)
                        continue;

                    var local = LocalizacoesMock.LocalizacoesFake[_random.Next(LocalizacoesMock.LocalizacoesFake.Count)];

                    var condominio = new Condominio
                    (
                        nome: $"Condominio {numero}",
                        senha: HashSenha.HashSenhaUsuario("123456"),
                        cnpjCpf: $"000000000000{numero}",
                        email: email,
                        logo: $"logoCondominio{numero}.png",
                        ativo: true,
                        rua: $"Rua Teste {numero}",
                        bairro: $"Bairro Teste",
                        cidade: local.Cidade,
                        estado: local.Estado,
                        cep: local.Cep
                    );

                    condominio.GerarCodigoVinculacao();
                    condominio.ContarCondominos();

                    condominios.Add(condominio);
                }

                foreach (var condominio in condominios)
                {
                    var novoCondominio = await _condominioRepository.CadastrarCondominioAsync(condominio);

                    var url = _configuration.GetSection("CotacaoApi")["CriarCondominio"];

                    if (string.IsNullOrEmpty(url))
                    {
                        await DeletarCondominioAsync(novoCondominio.Id);

                        return new RetornoGenerico(
                            false,
                            "URL da API de cotação não configurada",
                            "Erro interno",
                            HttpStatusCode.InternalServerError
                        );
                    }

                    var response = await _httpAppService.PostAsync(
                        url.Replace("{condominioId}", novoCondominio.Id.ToString())
                           .Replace("{nome}", novoCondominio.Nome),
                        null
                    );

                    if (response is null || !response.IsSuccessStatusCode)
                    {
                        await DeletarCondominioAsync(novoCondominio.Id);

                        return new RetornoGenerico(
                            false,
                            "Erro ao registrar condomínio na API de cotação",
                            "Erro ao criar condomínio de teste",
                            HttpStatusCode.BadRequest
                        );
                    }
                }

                retorno.Sucesso = true;
                retorno.HttpStatusCode = HttpStatusCode.OK;
                retorno.MensagemSistema = "Condomínios de teste cadastrados com sucesso";
                retorno.MensagemUsuario = "Condomínios criados com sucesso";

                return retorno;
            }
            catch (Exception ex)
            {
                retorno.Sucesso = false;
                retorno.HttpStatusCode = HttpStatusCode.InternalServerError;
                retorno.MensagemSistema = ex.ToString();
                retorno.MensagemUsuario = "Não foi possível criar condomínios de teste";
                retorno.Dados = null;

                return retorno;
            }
        }
    }
}
