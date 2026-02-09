using AutoMapper;
using Gerenciado_de_Usuario_Rapido_Facil.Application.DTOs;
using Gerenciado_de_Usuario_Rapido_Facil.Application.Interfaces;
using Gerenciado_de_Usuario_Rapido_Facil.Application.ViewModel;
using Gerenciado_de_Usuario_Rapido_Facil.CrossCutting.Extensions;
using Gerenciado_de_Usuario_Rapido_Facil.Domain.Entities;
using Gerenciado_de_Usuario_Rapido_Facil.Infra.Data.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace Gerenciado_de_Usuario_Rapido_Facil.Application.Services
{
    public class EmpresaPrestadoraAppService : IEmpresaPrestadoraAppService
    {
        private readonly IMapper _mapper;
        private readonly IServicoRepository _servicoRepository;
        private readonly IEmpresaPrestadoraRepository _empresaPrestadoraRepository;
        private readonly IValidacaoParaCadastroRepository _validacaoParaCadastroUsuario;
        private readonly IServicoAppService _servicoAppService;
        private readonly IConfiguration _configuration;
        private readonly IHttpAppService _httpAppService;
        private readonly IAnaliseIdoneidadeAppService _analiseIdoneidadeAppService;
        public EmpresaPrestadoraAppService(IMapper mapper,
            IServicoRepository servicoRepository,
            IEmpresaPrestadoraRepository empresaPrestadoraRepository,
            IValidacaoParaCadastroRepository validacaoParaCadastroRepository,
            IServicoAppService servicoAppService,
            IConfiguration configuration,
            IHttpAppService httpAppService,
            IAnaliseIdoneidadeAppService analiseIdoneidadeAppService
            )
        {
            _mapper = mapper;
            _servicoRepository = servicoRepository;
            _empresaPrestadoraRepository = empresaPrestadoraRepository;
            _validacaoParaCadastroUsuario = validacaoParaCadastroRepository;
            _servicoAppService = servicoAppService;
            _configuration = configuration;
            _httpAppService = httpAppService;
            _analiseIdoneidadeAppService = analiseIdoneidadeAppService;
        }

        public async Task<RetornoGenerico> CadastrarEmpresaAsync(EmpresaPrestadoraDTO empresaDTO)
        {
            var retorno = new RetornoGenerico();
            try
            {

                var valido = await _validacaoParaCadastroUsuario.VerificarEmailExiste(empresaDTO.Email);

                if (valido)
                {
                    return new RetornoGenerico(true,
                        "Este Email já esta cadastrado",
                        "Este Email já esta cadastrado",
                        HttpStatusCode.BadRequest
                        );
                }

                empresaDTO.Senha = HashSenha.HashSenhaUsuario(empresaDTO.Senha);

                var empresaPrestadora = _mapper.Map<EmpresaPrestadora>(empresaDTO);

                var novoEmpresaCadastrada = await _empresaPrestadoraRepository.CadastrarEmpresaAsync(empresaPrestadora);


                if (novoEmpresaCadastrada is null) return new RetornoGenerico(false,
                                                 "erro ao cadastrar empresa",
                                                  "erro ao cadastrar empresa",
                                                 HttpStatusCode.InternalServerError);

                var ulr = _configuration.GetSection("CotacaoApi")["CriarEmpresaPrestadora"];

                if (string.IsNullOrEmpty(ulr)) return new RetornoGenerico(false,
                                                 "URL do gerenciador de usuario não fornecedia",
                                                "URL do gerenciador de usuario não fornecedia",
                                                 HttpStatusCode.InternalServerError);

                if (novoEmpresaCadastrada != null)
                {
                    var response = await _httpAppService.PostAsync(ulr.Replace("{empresaPrestadoraId}", novoEmpresaCadastrada.Id.ToString()).Replace("{nome}", novoEmpresaCadastrada.Nome), null);

                    if (response is null || !response.IsSuccessStatusCode)
                    {
                        await DeletarEmpresaAsync(novoEmpresaCadastrada.Id);

                        return new RetornoGenerico(false,
                        "Não foi possivel enviar o id da empresa prestadora para a api de cotação",
                        "Não foi possivel enviar o id da empresa prestadora para a api de cotação",
                        HttpStatusCode.BadRequest,
                        null
                       );
                    }


                    await _analiseIdoneidadeAppService.CriarIdoneidade(novoEmpresaCadastrada.Id);
                }

                // Fazer analise de idoneidade

               await _analiseIdoneidadeAppService.CriarIdoneidade(novoEmpresaCadastrada.Id);

                retorno.Sucesso = novoEmpresaCadastrada != null ? true : false;
                retorno.HttpStatusCode = novoEmpresaCadastrada != null ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
                retorno.MensagemSistema = novoEmpresaCadastrada != null ? "Empresa cadastrada com sucesso" : "Empresa cadastrada com sucesso";
                retorno.MensagemUsuario = novoEmpresaCadastrada != null ? "Empresa cadastrada com sucesso" : "Empresa cadastrada com sucesso";

                return retorno;
            }
            catch (Exception ex)
            {
                retorno.Sucesso = false;
                retorno.HttpStatusCode = HttpStatusCode.InternalServerError;
                retorno.MensagemSistema = $"{ex}";
                retorno.MensagemUsuario = "Não foi possivel criar a Empresa Prestadora";
                retorno.Dados = null;
                return retorno;
            }
        }

        public async Task<RetornoGenerico> BuscarEmpresaAsync(Guid empresaId)
        {
            var empresa =  await _empresaPrestadoraRepository.BuscarEmpresa(empresaId);

            if (empresa is null)
            {
                return new RetornoGenerico
                {
                    Sucesso = false,
                    HttpStatusCode = HttpStatusCode.NotFound,
                    MensagemSistema = "Empresa nao encontrada",
                    MensagemUsuario = "Empresa nao encontrada",
                    Dados = null
                };
            }

            //var EmpresaPrestadoraServicoSubtipos = empresa.EmpresaPrestadoraServicoSubtipos
            //    .Where(servico => servico?.Servico != null)
            //    .Select(servico => new EmpresaPrestadoraServicoSubtipoViewModel
            //    {
            //        Id = servico.ServicoId,
            //        Nome = servico.Servico.Nome ?? string.Empty, 
            //        ServicoSubtipos = servico.Servico.ServicoSubtipos?
            //            .Where(subservico => subservico != null) 
            //            .Select(subservico => new ServicoSubtipoViewModel { Nome = subservico.Nome, EmpresaPrestadoraId = servico.EmpresaPrestadoraId, ServicoId = servico.ServicoId, ServicoSubtipoId = subservico.Id })
            //            .ToList() ?? new List<ServicoSubtipoViewModel>()
            //    })
            //    .ToList();


            return new RetornoGenerico
            {
                Sucesso = true,
                HttpStatusCode = HttpStatusCode.OK,
                MensagemSistema = "Empresa encontrada",
                MensagemUsuario = "Empresa encontrada",
                Dados = empresa
            };
        }

        public async Task<RetornoGenerico> BuscarEmpresaCompletoAsync(Guid empresaId)
        {
            var Empresa = await _empresaPrestadoraRepository.BuscarEmpresaCompleto(empresaId);

            var mensagemSistema = Empresa == null ? "empresa nãa encontrada" : "empresa encontrada";
            var mensagemUsuario = Empresa == null ? "empresa nãa encontrada" : "empresa encontrada";

            var resultado = Empresa == null ? false : true;

            return new RetornoGenerico
            {
                Sucesso = resultado,
                HttpStatusCode = resultado ? HttpStatusCode.OK : HttpStatusCode.NotFound,
                MensagemSistema = mensagemSistema,
                MensagemUsuario = mensagemUsuario,
                Dados = Empresa
            };
        }

        public async Task<RetornoGenerico> BuscarTodosAsEmpresasAsync()
        {
            var empresa = await _empresaPrestadoraRepository.BuscarTodosOsEmpresaAsync();

            var mensagem = empresa.Count > 0 ? $"{empresa.Count} empresas cadastradas" : "Não há empresas cadastradas";
            var sucesso = empresa.Count > 0;

            return new RetornoGenerico
            {
                Sucesso = sucesso,
                HttpStatusCode =  HttpStatusCode.OK,
                MensagemSistema = mensagem,
                MensagemUsuario = mensagem,
                Dados = empresa
            };
        }

        public async Task<RetornoGenerico> DeletarEmpresaAsync(Guid empresaId)
        {
            var buscaCondominio = await BuscarEmpresaCompletoAsync(empresaId);

            if (!buscaCondominio.Sucesso)
            {

                return new RetornoGenerico
                {
                    Sucesso = buscaCondominio.Sucesso,
                    HttpStatusCode = HttpStatusCode.NotFound,
                    MensagemSistema = "Empresa não localizado",
                    MensagemUsuario = "Empresa não localizado",
                    Dados = null
                };
            }

            await _empresaPrestadoraRepository.DeletarEmpresaAsync(buscaCondominio.Dados);

            return new RetornoGenerico
            {
                Sucesso = true,
                HttpStatusCode = HttpStatusCode.OK,
                MensagemSistema = "Empresa Deletado",
                MensagemUsuario = "Empresa Deletado",
                Dados = null
            };
        }

        public async Task<RetornoGenerico> EditarEmpresaAsync(Guid empresaId, EmpresaPrestadoraAtualizacaoDTO edicaoEmpresaDTO)
        {
            var buscaEmpresa = await BuscarEmpresaCompletoAsync(empresaId);

            if (!buscaEmpresa.Sucesso)
            {
                return new RetornoGenerico
                {
                    Sucesso = buscaEmpresa.Sucesso,
                    HttpStatusCode = HttpStatusCode.NotFound,
                    MensagemSistema = "Empresa não localizado",
                    MensagemUsuario = "Empresa não localizado",
                    Dados = null
                };
            }

            EmpresaPrestadora empresa = buscaEmpresa.Dados;

            var dadosAtualizados = new EmpresaPrestadora(
                edicaoEmpresaDTO.Nome ?? empresa.Nome,
                empresa.Senha,
                edicaoEmpresaDTO.CnpjCpf ?? empresa.CnpjCpf,
                edicaoEmpresaDTO.Email ?? empresa.Email,
                edicaoEmpresaDTO.Logo ?? empresa.Logo,
                edicaoEmpresaDTO.Ativo ?? empresa.Ativo,
                edicaoEmpresaDTO.Rua ?? empresa.Rua,
                edicaoEmpresaDTO.Bairro ?? empresa.Bairro,
                edicaoEmpresaDTO.Cidade ?? empresa.Cidade,
                edicaoEmpresaDTO.Estado ?? empresa.Estado,
                edicaoEmpresaDTO.Cep ?? empresa.Cep
            );

            empresa.EditarEmpresaPrestadora(dadosAtualizados);

            await _empresaPrestadoraRepository.EditarEmpresaAsync(empresa);


            return new RetornoGenerico
            {
                Sucesso = true,
                HttpStatusCode = HttpStatusCode.OK,
                MensagemSistema = "Empresa atualizado com sucesso",
                MensagemUsuario = "Empresa atualizado com sucesso",
                Dados = null
            };
        }

        public async Task<RetornoGenerico> BuscarPrestadorPorFiltros(BuscarPrestadorPorFiltrosDTO filtros)
        {
            var empresas = await _empresaPrestadoraRepository.BuscarPrestadorPorFiltros(filtros.nome, filtros.cidade, filtros.estado, filtros.pagina, filtros.itensPorPagina, filtros.servicoId, filtros.servicoSubtipoIds);

            var mensagem = empresas.TotalItems > 0 ? $"{empresas.TotalItems} empresas encontrada" : "Não há empresas cadastradas";

            return new RetornoGenerico
            {
                Sucesso = true,
                HttpStatusCode = HttpStatusCode.OK,
                MensagemSistema = mensagem,
                MensagemUsuario = mensagem,
                Dados = empresas
            };
        }

        public async Task<RetornoGenerico> FinalizarCadastroEmpresaPrestadora(Guid empresaId, FinalizarCadastroEmpresaPrestadora finalizarCadastro)
        {
            var buscaEmpresa = await BuscarEmpresaCompletoAsync(empresaId);

            if (!buscaEmpresa.Sucesso)
            {

                return new RetornoGenerico
                {
                    Sucesso = buscaEmpresa.Sucesso,
                    HttpStatusCode = HttpStatusCode.NotFound,
                    MensagemSistema = "Empresa não localizado",
                    MensagemUsuario = "Empresa não localizado",
                    Dados = null
                };
            }

            EmpresaPrestadora empresa = buscaEmpresa.Dados;

            var dadosAtualizados = new EmpresaPrestadora(
                finalizarCadastro.Nome ?? empresa.Nome,
                empresa.Senha,
                finalizarCadastro.CnpjCpf ?? empresa.CnpjCpf,
                finalizarCadastro.Email ?? empresa.Email,
                finalizarCadastro.Logo ?? empresa.Logo,
                empresa.Ativo,
                finalizarCadastro.Rua ?? empresa.Rua,
                finalizarCadastro.Bairro ?? empresa.Bairro,
                finalizarCadastro.Cidade ?? empresa.Cidade,
                finalizarCadastro.Estado ?? empresa.Estado,
                finalizarCadastro.Cep ?? empresa.Cep
            );

            empresa.EditarEmpresaPrestadora(dadosAtualizados);

            await _empresaPrestadoraRepository.EditarEmpresaAsync(empresa);

            foreach (var item in finalizarCadastro.Servicos)
            {
                item.EmpresaPrestadoraId = empresaId;
            }

            await _servicoAppService.VincularServicoAEmpresa(empresaId, finalizarCadastro.Servicos);


            return new RetornoGenerico
            {
                Sucesso = true,
                HttpStatusCode = HttpStatusCode.OK,
                MensagemSistema = "Empresa atualizado com sucesso",
                MensagemUsuario = "Empresa atualizado com sucesso",
                Dados = null
            };
        }

        public async Task<dynamic> BuscarEmpresaPorEmailAsync(string email)
        {
           var empresaPrestadora = await _empresaPrestadoraRepository.BuscarPrestadorPorEmail(email);

            return empresaPrestadora;
        }

        public async Task<RetornoGenerico> CadastrarEmpresasParaTesteAsync(int numeroDePrestadores)
        {
            var retorno = new RetornoGenerico();
            try
            {
                var empresasPrestadoras = new List<EmpresaPrestadora>();


                var random = new Random();
                var list = await _servicoRepository.BuscarTodosOsServicos();
  

                for (int i = 1; i <= numeroDePrestadores; i++)
                {
                    string numeroDoPrestador = i.ToString("D2");

                    var valido = await _validacaoParaCadastroUsuario.VerificarEmailExiste($"prestador{numeroDoPrestador}@gmail.com");

                    if (valido)
                    {
                        continue;
                    }

                    var empresa = new EmpresaPrestadora(
                        nome: $"Prestador{numeroDoPrestador}",
                        senha: HashSenha.HashSenhaUsuario("123456"),
                        cnpjCpf: $"00000000000{numeroDoPrestador}",
                        email: $"prestador{numeroDoPrestador}@gmail.com",
                        logo: $"logoPrestador{numeroDoPrestador}.png",
                        ativo: true,
                        rua: $"RuaPrestador{numeroDoPrestador}",
                        bairro: $"BairroPrestador{numeroDoPrestador}",
                        cidade: $"CidadePrestador{numeroDoPrestador}",
                        estado: $"EstadoPrestador{numeroDoPrestador}",
                        cep: $"00000{numeroDoPrestador}"
                    );

                    empresasPrestadoras.Add(empresa);
                }

                foreach (var empresa in empresasPrestadoras)
                {
                    var novoEmpresaCadastrada = await _empresaPrestadoraRepository.CadastrarEmpresaAsync(empresa);

                    var servicoEsubservicoSorteado = list[random.Next(list.Count)];

                    var empresaPrestadoraServicoSubtipoDTOs = new List<EmpresaPrestadoraServicoSubtipoDTO>();

                    foreach (var subTipos in servicoEsubservicoSorteado.ServicoSubtipos)
                    {
                        var novoServicoSubtipoDTO = new EmpresaPrestadoraServicoSubtipoDTO()
                        {
                            EmpresaPrestadoraId = novoEmpresaCadastrada.Id,
                            ServicoId = servicoEsubservicoSorteado.Id,
                            ServicoSubtipoId = subTipos.Id
                        };

                        empresaPrestadoraServicoSubtipoDTOs.Add(novoServicoSubtipoDTO);
                    }

     
                    await _servicoAppService.VincularServicoAEmpresa(novoEmpresaCadastrada.Id, empresaPrestadoraServicoSubtipoDTOs);


                    var ulr = _configuration.GetSection("CotacaoApi")["CriarEmpresaPrestadora"];

                    if (string.IsNullOrEmpty(ulr)) return new RetornoGenerico(false,
                                                     "URL do gerenciador de usuario não fornecedia",
                                                    "URL do gerenciador de usuario não fornecedia",
                                                     HttpStatusCode.InternalServerError);

                    if (novoEmpresaCadastrada != null)
                    {
                        var response = await _httpAppService.PostAsync(ulr.Replace("{empresaPrestadoraId}", novoEmpresaCadastrada.Id.ToString()).Replace("{nome}", novoEmpresaCadastrada.Nome), null);

                        if (response is null || !response.IsSuccessStatusCode)
                        {
                            await DeletarEmpresaAsync(novoEmpresaCadastrada.Id);

                            return new RetornoGenerico(false,
                            $"Não foi possivel enviar o id da {empresa.Nome}  para a api de cotação",
                            $"Não foi possivel enviar o id da {empresa.Nome}  para a api de cotação",
                            HttpStatusCode.BadRequest,
                            null
                           );
                        }

                        await _analiseIdoneidadeAppService.CriarIdoneidade(novoEmpresaCadastrada.Id);
                    }
                }


                retorno.Sucesso =  true;
                retorno.HttpStatusCode = HttpStatusCode.OK;
                retorno.MensagemSistema = "Empresas cadastradas com sucesso";
                retorno.MensagemUsuario = "Empresas cadastradas com sucesso" ;

                return retorno;
            }
            catch (Exception ex)
            {
                retorno.Sucesso = false;
                retorno.HttpStatusCode = HttpStatusCode.InternalServerError;
                retorno.MensagemSistema = $"{ex}";
                retorno.MensagemUsuario = "Não foi possivel criar a Empresa Prestadora";
                retorno.Dados = null;
                return retorno;
            }
        }

        public async Task<RetornoGenerico> VincularEmpresaAServicoSubServico(Guid empresaId, List<EmpresaPrestadoraServicoSubtipoDTO> listaServicosSubServicos)
        {
            var buscaEmpresa = await BuscarEmpresaCompletoAsync(empresaId);

            if (!buscaEmpresa.Sucesso)
            {
                return new RetornoGenerico
                {
                    Sucesso = buscaEmpresa.Sucesso,
                    HttpStatusCode = HttpStatusCode.NotFound,
                    MensagemSistema = "Empresa não localizado",
                    MensagemUsuario = "Empresa não localizado",
                    Dados = null
                };
            }

            await _servicoAppService.VincularServicoAEmpresa(empresaId, listaServicosSubServicos);


            return new RetornoGenerico
            {
                Sucesso = true,
                HttpStatusCode = HttpStatusCode.OK,
                MensagemSistema = "Empresa atualizada com sucesso",
                MensagemUsuario = "Empresa atualizada com sucesso",
                Dados = null
            };
        }

        public async Task<RetornoGenerico> AgruparInformacoesPrestador(Guid empresaId)
        {
            var Empresa = await _empresaPrestadoraRepository.BuscarEmpresa(empresaId);

            if (Empresa is null) {
                return new RetornoGenerico
                {
                    Sucesso = false,
                    HttpStatusCode = HttpStatusCode.NotFound,
                    MensagemSistema = "Prestador não encontrado",
                    MensagemUsuario = "Prestador não encontrado",
                    Dados = null
                };
            }


            var informacoes = new InformacoesPrestador()
            {
                Nome = Empresa.Nome,
                DataCadastro = Empresa.DataCadastro,
                QtServicoPrestados = 0,
                Verificada = true,
                EmpresaPrestadoraServicoSubtipos = Empresa.EmpresaPrestadoraServicoSubtipos,
            };

            return new RetornoGenerico
            {
                Sucesso = true,
                HttpStatusCode = HttpStatusCode.OK,
                MensagemSistema = "informações reunidas",
                MensagemUsuario = "informações reunidas",
                Dados = informacoes
            };
        }

        public async Task<RetornoGenerico> TrocarSenha(Guid empresaId, TrocaSenhaDTO trocaSenhaDTO)
        {
            var ulr = _configuration.GetSection("AutenticacaoApi")["autenticacaoEmpresaPrestadora"];

            if (string.IsNullOrEmpty(ulr)) return new RetornoGenerico(false,
                                                "URL para autenticação  de usuario não fornecida",
                                                "URL para autenticação  de usuario não fornecida",
                                                HttpStatusCode.BadRequest
                                            );
            var buscaEmpresa = await BuscarEmpresaCompletoAsync(empresaId);

            if (!buscaEmpresa.Sucesso)
            {
                return new RetornoGenerico(false,
                      "empresa não encontrada",
                      "empresa não encontrada",
                      HttpStatusCode.NotFound,
                      null
                 );
            }

            EmpresaPrestadora empresa = buscaEmpresa.Dados;

            var dto = new LoginDTO()
            {
                Email = buscaEmpresa.Dados.Email,
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

            empresa.Senha = HashSenha.HashSenhaUsuario(trocaSenhaDTO.SenhaNova);

            await _empresaPrestadoraRepository.EditarEmpresaAsync(empresa);


            return new RetornoGenerico(true,
                    "Senha atualizada com sucesso",
                    "Senha atualizada com sucesso",
                    HttpStatusCode.OK,
                    null
                );
        }
    }
}
