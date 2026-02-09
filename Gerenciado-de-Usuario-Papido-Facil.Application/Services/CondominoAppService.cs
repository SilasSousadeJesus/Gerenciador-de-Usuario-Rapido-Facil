using AutoMapper;
using Gerenciado_de_Usuario_Papido_Facil.Application.DTOs;
using Gerenciado_de_Usuario_Papido_Facil.Application.Interfaces;
using Gerenciado_de_Usuario_Papido_Facil.Application.ViewModel;
using Gerenciado_de_Usuario_Papido_Facil.CrossCutting.Extensions;
using Gerenciado_de_Usuario_Papido_Facil.Domain.Entities;
using Gerenciado_de_Usuario_Papido_Facil.Infra.Data.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace Gerenciado_de_Usuario_Papido_Facil.Application.Services
{
    public class CondominoAppService : ICondominoAppService
    {
        private readonly IMapper _mapper;
        private readonly ICondominioRepository  _condominioRepository;
        private readonly ICondominioAppService   _condominioAppService;
        private readonly ICondominoRepository _CondominoRepository;
        private readonly IValidacaoParaCadastroRepository _validacaoParaCadastroUsuario;
        private readonly IConfiguration _configuration;
        private readonly IHttpAppService _httpAppService;
        public CondominoAppService(
            IMapper mapper, 
            ICondominioRepository condominioRepository,
            ICondominioAppService condominioAppService,
            ICondominoRepository condominoRepository,
            IValidacaoParaCadastroRepository validacaoParaCadastroRepository,
            IConfiguration configuration,
            IHttpAppService httpAppService)
        {
            _mapper = mapper;
            _condominioRepository = condominioRepository;
            _condominioAppService = condominioAppService;
            _CondominoRepository = condominoRepository;
            _validacaoParaCadastroUsuario = validacaoParaCadastroRepository;
            _configuration = configuration;
            _httpAppService = httpAppService;
        }

        public async Task<RetornoGenerico> CadastrarCondominoAsync(CadastroCondominoDTO condominoDTO)
        {
            var retorno = new RetornoGenerico();
            try
            {

                var valido = await _validacaoParaCadastroUsuario.VerificarEmailExiste(condominoDTO.Email);

                if (valido)
                {
                    return new RetornoGenerico(true,
                        "Este Email já esta cadastrado",
                        "Este Email já esta cadastrado",
                        HttpStatusCode.BadRequest
                        );
                }
                
                condominoDTO.Senha = HashSenha.HashSenhaUsuario(condominoDTO.Senha);

                var condomino = _mapper.Map<Condomino>(condominoDTO);

                var novoCondominoCadastrado =  await _CondominoRepository.CadastrarCondominoAsync(condomino);

                var ulr = _configuration.GetSection("CotacaoApi")["CriarCondomino"];

                if (string.IsNullOrEmpty(ulr)) return new RetornoGenerico(false,
                                                 "URL do gerenciador de usuario não fornecedia",
                                                "URL do gerenciador de usuario não fornecedia",
                                                 HttpStatusCode.InternalServerError
                                             );

                if (novoCondominoCadastrado != null)
                {
                    var response = await _httpAppService.PostAsync(ulr.Replace("{condominoId}", novoCondominoCadastrado.Id.ToString()).Replace("{nome}", novoCondominoCadastrado.Nome), null);

                    if (response is null || !response.IsSuccessStatusCode)
                    {
                        await DeletarCondominoAsync(novoCondominoCadastrado.Id);

                        return new RetornoGenerico(false,
                        "Não foi possivel enviar o id do condomino para a api de cotação",
                         "Não foi possivel enviar o id do condomino para a api de cotação",
                        HttpStatusCode.BadRequest,
                        null
                       );
                    }
                }

                if (!string.IsNullOrEmpty(condominoDTO.CodigoVinculo))
                {
                    await VincularCondominoACondominioAsync(condominoDTO.CodigoVinculo, novoCondominoCadastrado.Id);
                }

                return new RetornoGenerico(true,
                    "Condomino criado",
                    "O cadastro do seu Condomino foi feito com sucesso",
                    HttpStatusCode.Created
                );
            }
            catch (Exception ex)
            {
                retorno.Sucesso = false;
                retorno.HttpStatusCode = HttpStatusCode.InternalServerError;
                retorno.MensagemSistema = $"{ex}";
                retorno.MensagemUsuario = "Não foi possivel criar um Condomino";
                retorno.Dados = null;
                return retorno;
            }
        }
        public async Task<RetornoGenerico> VincularCondominoACondominioAsync(string codigo, Guid condominoId)
        {
            Condominio? condiminioVinculado = await _condominioRepository.BuscarPorCodigoVinculacaoAsync(codigo);
            var buscaCondomino = await BuscarUmCondominoCompletoAsync(condominoId);

            if (condiminioVinculado is null || buscaCondomino.Dados  is null)
            {
                return new RetornoGenerico(true,
                                      "Condominio ou Condomino não encontrado",
                                      "Condominio ou Condomino não encontrado",
                                       HttpStatusCode.NotFound
                                     );
            }

            buscaCondomino.Dados!.Vinculado = true;
            buscaCondomino.Dados.Rua = condiminioVinculado!.Rua;
            buscaCondomino.Dados.Bairro = condiminioVinculado!.Bairro;
            buscaCondomino.Dados.Cidade = condiminioVinculado!.Cidade;
            buscaCondomino.Dados.Estado = condiminioVinculado!.Estado;
            buscaCondomino.Dados.Cep = condiminioVinculado!.Cep;


            await _CondominoRepository.EditarCondominoAsync(buscaCondomino.Dados);

            var condiminio = await _condominioRepository.BuscarUmCondominioCompletoAsync(condiminioVinculado!.Id);

            condiminio.ContarCondominos();

            await _condominioRepository.EditarCondominioAsync(condiminio);

            return new RetornoGenerico
            {
                Sucesso = true,
                HttpStatusCode = HttpStatusCode.OK,
                MensagemSistema = "condomino vinculado com sucesso",
                MensagemUsuario = "condomino vinculado com sucesso",
                Dados = null
            };
        }
        public async Task<RetornoGenerico> BuscarUmCondominoAsync(Guid condominoId)
        {
            var condomino = await _CondominoRepository.BuscarUmCondominoAsync(condominoId);

            if (condomino == null)
            {
                return new RetornoGenerico
                {
                    Sucesso = false,
                    HttpStatusCode = HttpStatusCode.NotFound,
                    MensagemSistema = "Condomino não encontrado",
                    MensagemUsuario = "Condomino não encontrado",
                    Dados = null
                };
            }

            CondominioViewModel? condominioViewModel = null;

            if (condomino.CondominioId.HasValue)
            {
                var condominio = await _condominioRepository
                    .BuscarUmCondominioAsync((Guid)condomino.CondominioId);

                if (condominio != null)
                {
                    condominioViewModel = new CondominioViewModel
                    {
                        Id = condominio.Id,
                        Nome = condominio.Nome,
                        CnpjCpf = condominio.CnpjCpf,
                        Email = condominio.Email,
                        Ativo = condominio.Ativo,
                        CodigoVinculacao = condominio.CodigoVinculacao,
                        Rua = condominio.Rua,
                        Bairro = condominio.Bairro,
                        Cidade = condominio.Cidade,
                        Estado = condominio.Estado,
                        Cep = condominio.Cep,
                        Senha = string.Empty
                    };
                }
            }

            var condominoViewModel = new CondominoViewModel
            {
                Id = condomino.Id,
                Nome = condomino.Nome,
                CnpjCpf = condomino.CnpjCpf,
                Email = condomino.Email,
                Senha = string.Empty,
                Ativo = condomino.Ativo,
                Rua = condomino.Rua,
                Bairro = condomino.Bairro,
                Cidade = condomino.Cidade,
                Estado = condomino.Estado,
                Cep = condomino.Cep,
                Vinculado = condomino.Vinculado,
                CodigoVinculacao = condomino.CodigoVinculacao,
                CondominioId = condomino.CondominioId,
                Condominio = condominioViewModel
            };

            return new RetornoGenerico
            {
                Sucesso = true,
                HttpStatusCode = HttpStatusCode.OK,
                MensagemSistema = "Condomino encontrado",
                MensagemUsuario = "Condomino encontrado",
                Dados = condominoViewModel
            };
        }

        public async Task<RetornoGenerico> BuscarUmCondominoCompletoAsync(Guid condominoId)
        {
            var condomino = await _CondominoRepository.BuscarUmCondominoCompletoAsync(condominoId);

            var mensagemSistema = condomino == null ? "condomino não encontrado" : "condomino Encontrado";
            var mensagemUsuario = condomino == null ? "condomino não encontrado" : "condomino Encontrado";

            var resultado = condomino == null ? false : true;

            return new RetornoGenerico
            {
                Sucesso = resultado,
                HttpStatusCode = resultado ? HttpStatusCode.OK : HttpStatusCode.NotFound,
                MensagemSistema = mensagemSistema,
                MensagemUsuario = mensagemUsuario,
                Dados = condomino
            };
        }
        public async Task<RetornoGenerico> BuscarTodosOsCondominosAsync(Guid condominioId, BuscarCondominoPorFiltrosDTO filtros)
        {
            var condominos = await _CondominoRepository.BuscarTodosOsCondominosAsync(condominioId, filtros.nome, filtros.email, filtros.ativo, filtros.dataCadastro, filtros.pagina, filtros.itensPorPagina);

            var mensagem = condominos.TotalItems > 0 ? $"{condominos.TotalItems} condominos encontrados" : "Não há condominos cadastrados";

            return new RetornoGenerico
            {
                Sucesso = true,
                HttpStatusCode = HttpStatusCode.OK,
                MensagemSistema = mensagem,
                MensagemUsuario = mensagem,
                Dados = condominos
            };
        }
        public async Task<RetornoGenerico> DeletarCondominoAsync(Guid condominoId)
        {
            var buscaCondominio = await BuscarUmCondominoAsync(condominoId);

            if (!buscaCondominio.Sucesso) {

                return new RetornoGenerico
                {
                    Sucesso = buscaCondominio.Sucesso,
                    HttpStatusCode = HttpStatusCode.NotFound,
                    MensagemSistema = "Condominio não localizado",
                    MensagemUsuario = "Condominio não localizado",
                    Dados = null
                };
            }

            await _CondominoRepository.DeletarCondominoAsync(buscaCondominio.Dados);

            return new RetornoGenerico
            {
                Sucesso = true,
                HttpStatusCode = HttpStatusCode.OK,
                MensagemSistema = "Condomino Deletado",
                MensagemUsuario = "Condomino Deletado",
                Dados = null
            };
        }
        public async Task<RetornoGenerico> EditarCondominoAsync(Guid condominoId, EdicaoCondominoDTO edicaoCondominoDTO)
        {
            var buscaCondominio = await BuscarUmCondominoCompletoAsync(condominoId);

            if (!buscaCondominio.Sucesso)
            {
                return new RetornoGenerico
                {
                    Sucesso = buscaCondominio.Sucesso,
                    HttpStatusCode = HttpStatusCode.NotFound,
                    MensagemSistema = "Condomínio não localizado",
                    MensagemUsuario = "Condomínio não localizado",
                    Dados = null
                };
            }

            Condomino condomino = buscaCondominio.Dados;

            if (!string.IsNullOrWhiteSpace(edicaoCondominoDTO.Nome))
                condomino.Nome = edicaoCondominoDTO.Nome;

            if (!string.IsNullOrWhiteSpace(edicaoCondominoDTO.CnpjCpf))
                condomino.CnpjCpf = edicaoCondominoDTO.CnpjCpf;

            if (!string.IsNullOrWhiteSpace(edicaoCondominoDTO.Email))
                condomino.Email = edicaoCondominoDTO.Email;

            if (!string.IsNullOrWhiteSpace(edicaoCondominoDTO.Logo))
                condomino.Logo = edicaoCondominoDTO.Logo;

            if (edicaoCondominoDTO.Ativo.HasValue)
                condomino.Ativo = edicaoCondominoDTO.Ativo.Value;

            //if (!string.IsNullOrWhiteSpace(edicaoCondominoDTO.Rua))
            //    condomino.Rua = edicaoCondominoDTO.Rua;

            //if (!string.IsNullOrWhiteSpace(edicaoCondominoDTO.Bairro))
            //    condomino.Bairro = edicaoCondominoDTO.Bairro;

            //if (!string.IsNullOrWhiteSpace(edicaoCondominoDTO.Cidade))
            //    condomino.Cidade = edicaoCondominoDTO.Cidade;

            //if (!string.IsNullOrWhiteSpace(edicaoCondominoDTO.Estado))
            //    condomino.Estado = edicaoCondominoDTO.Estado;

            //if (!string.IsNullOrWhiteSpace(edicaoCondominoDTO.Cep))
            //    condomino.Cep = edicaoCondominoDTO.Cep;

            if (!string.IsNullOrWhiteSpace(edicaoCondominoDTO.Apartamento))
                condomino.Apartamento = edicaoCondominoDTO.Apartamento;

            await _CondominoRepository.EditarCondominoAsync(condomino);

            return new RetornoGenerico
            {
                Sucesso = true,
                HttpStatusCode = HttpStatusCode.OK,
                MensagemSistema = "Condomino atualizado com sucesso",
                MensagemUsuario = "Condomino atualizado com sucesso",
                Dados = null
            };
        }

        public async Task<CondominoViewModel?> BuscarUmCondominoPorEmailAsync(string email)
        {
            var condomino = await _CondominoRepository.BuscarCondominoPorEmail(email);

            if (condomino is null) return null;

            var condominoViewModel = _mapper.Map<CondominoViewModel>(condomino);

            return condominoViewModel;
        }

        public async Task<RetornoGenerico> TrocarSenha(Guid condominoId, TrocaSenhaDTO trocaSenhaDTO)
        {
            var ulr = _configuration.GetSection("AutenticacaoApi")["autenticacaoCondomino"];

            if (string.IsNullOrEmpty(ulr)) return new RetornoGenerico(false,
                                                "URL para autenticação  de usuario não fornecida",
                                                "URL para autenticação  de usuario não fornecida",
                                                HttpStatusCode.BadRequest
                                            );
            var buscaCondomino = await BuscarUmCondominoCompletoAsync(condominoId);

            if (!buscaCondomino.Sucesso)
            {
                return new RetornoGenerico(false,
                      "Condomino não encontrado",
                      "Condomino não encontrado",
                      HttpStatusCode.NotFound,
                      null
                 );
            }

            Condomino condomino = buscaCondomino.Dados;

            var dto = new LoginDTO()
            {
                Email = buscaCondomino.Dados.Email,
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

            condomino.Senha = HashSenha.HashSenhaUsuario(trocaSenhaDTO.SenhaNova);

            await _CondominoRepository.EditarCondominoAsync(condomino);


            return new RetornoGenerico(true,
                    "Senha atualizada com sucesso",
                    "Senha atualizada com sucesso",
                    HttpStatusCode.OK,
                    null
                );
        }

        public async Task<RetornoGenerico> CadastrarCondominoParaTesteAsync(
            Guid condominioId,
            int numeroDeCondomino
        )
        {
            var retorno = new RetornoGenerico();

            try
            {
               var buscarCondominio = await _condominioAppService.BuscarUmCondominioAsync(condominioId);

                Condominio? condominio =  buscarCondominio.Dados;

                if (condominio is null)
                {
                    retorno.Sucesso = false;
                    retorno.HttpStatusCode = HttpStatusCode.NotFound;
                    retorno.MensagemSistema = "Condominio não encontrado";
                    retorno.MensagemUsuario = "Condominio não encontrado";

                    return retorno;
                }

                var condominos = new List<Condomino>();

                for (int i = 1; i <= numeroDeCondomino; i++)
                {
                    var numero = i.ToString("D2");
                    var email = $"condomino{numero}@gmail.com";

                    var valido = await _validacaoParaCadastroUsuario.VerificarEmailExiste(email);

                    if (valido)
                    {
                        continue;
                    }

                    var condomino = new Condomino(
                        nome: $"Condomino{numero}",
                        senha: HashSenha.HashSenhaUsuario("123456"),
                        cnpjCpf: $"000000000{numero}",
                        email: email,
                        logo: $"logoCondomino{numero}.png",
                        ativo: true,
                        rua: condominio.Rua,
                        bairro: condominio.Bairro,
                        cidade: condominio.Cidade,
                        estado: condominio.Estado,
                        cep: condominio.Cep,
                        apartamento: $"Apto-{numero}"
                    )
                    {
                        CondominioId = condominioId,
                        Vinculado = true,
                        CodigoVinculacao = condominio.CodigoVinculacao,
                        PlanoValido = true,
                        AceitouTermosDeUso = true
                    };

                    condominos.Add(condomino);
                }

                foreach (var condomino in condominos)
                {
                    var novoCondomino = await _CondominoRepository
                        .CadastrarCondominoAsync(condomino);

                    var url = _configuration
                        .GetSection("CotacaoApi")["CriarCondomino"];

                    if (string.IsNullOrEmpty(url))
                    {
                        await DeletarCondominoAsync(novoCondomino.Id);

                        return new RetornoGenerico(
                            false,
                            "URL da API de cotação não configurada",
                            "Erro interno",
                            HttpStatusCode.InternalServerError
                        );
                    }

                    var response = await _httpAppService.PostAsync(
                        url.Replace("{condominoId}", novoCondomino.Id.ToString())
                           .Replace("{nome}", novoCondomino.Nome),
                        null
                    );

                    if (response is null || !response.IsSuccessStatusCode)
                    {
                        await DeletarCondominoAsync(novoCondomino.Id);

                        return new RetornoGenerico(
                            false,
                            "Erro ao registrar condômino na API de cotação",
                            "Erro ao criar condômino de teste",
                            HttpStatusCode.BadRequest
                        );
                    }
                }

                retorno.Sucesso = true;
                retorno.HttpStatusCode = HttpStatusCode.OK;
                retorno.MensagemSistema = "Condôminos de teste cadastrados com sucesso";
                retorno.MensagemUsuario = "Condôminos criados com sucesso";

                return retorno;
            }
            catch (Exception ex)
            {
                retorno.Sucesso = false;
                retorno.HttpStatusCode = HttpStatusCode.InternalServerError;
                retorno.MensagemSistema = ex.ToString();
                retorno.MensagemUsuario = "Não foi possível criar condôminos de teste";
                retorno.Dados = null;

                return retorno;
            }
        }

    }
}
