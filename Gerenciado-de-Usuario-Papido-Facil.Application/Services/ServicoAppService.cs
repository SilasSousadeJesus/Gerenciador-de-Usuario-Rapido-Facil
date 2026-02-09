using AutoMapper;
using Gerenciado_de_Usuario_Papido_Facil.Application.DTOs;
using Gerenciado_de_Usuario_Papido_Facil.Application.Interfaces;
using Gerenciado_de_Usuario_Papido_Facil.Application.Resources.Classes;
using Gerenciado_de_Usuario_Papido_Facil.CrossCutting.Extensions;
using Gerenciado_de_Usuario_Papido_Facil.Domain.Entities;
using Gerenciado_de_Usuario_Papido_Facil.Infra.Data.Interfaces;
using System.Net;

namespace Gerenciado_de_Usuario_Papido_Facil.Application.Services
{
    public class ServicoAppService : IServicoAppService
    {
        private readonly IMapper _mapper;
        private readonly IServicoRepository _servicoRepository;
        private readonly IEmpresaPrestadoraRepository _empresaPrestadoraRepository;
        public ServicoAppService(IMapper mapper, 
            IServicoRepository servicoRepository, 
            IEmpresaPrestadoraRepository empresaPrestadoraRepository)
        {
            _mapper = mapper;
            _servicoRepository = servicoRepository;
            _empresaPrestadoraRepository = empresaPrestadoraRepository;
        }
        public async Task<RetornoGenerico> GeracaoDeServicoServicoSubTipoAsync()
        {
            var retorno = new RetornoGenerico();
            try
            {
                var fabricaServicos = new ServicosESubTipos();

                var list = fabricaServicos.ObterServicos();

                var resultado = await _servicoRepository.GeracaoDeServicoServicoSubTipoAsync(list);

                return new RetornoGenerico(resultado,
                    resultado ? "Serviços e SubTipo de Serviços Criados com sucesso" : "Não foi possivel criar os serviços",
                    resultado ? "Serviços e SubTipo de Serviços Criados com sucesso" : "Não foi possivel criar os serviços",
                    resultado ? HttpStatusCode.Created : HttpStatusCode.BadRequest
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

        public async Task<RetornoGenerico> BuscarTodosOsServicosAsync()
        {
            var servicos = await _servicoRepository.BuscarTodosOsServicos();

            var mensagem = servicos.Count > 0 ? $"{servicos.Count} serviços cadastrados" : "Não há serviços cadastrados";

            return new RetornoGenerico
            {
                Sucesso = true,
                HttpStatusCode = HttpStatusCode.OK,
                MensagemSistema = mensagem,
                MensagemUsuario = mensagem,
                Dados = servicos
            };
        }

        //public async Task<RetornoGenerico> VincularServicoAEmpresa(Guid empresaId, List<EmpresaPrestadoraServicoSubtipoDTO> listaEmpresaPrestadoraServicoSubtipoDTO)
        //{
        //    var retorno = new RetornoGenerico();
        //    try
        //    {

        //        // busco a empresa prestadora de serviço

        //        var empresa =  await _empresaPrestadoraRepository.BuscarEmpresaCompleto(empresaId);

        //        if (empresa is null)
        //        {
        //            retorno.Sucesso = false;
        //            retorno.HttpStatusCode = HttpStatusCode.NotFound;
        //            retorno.MensagemSistema = "Empresa não encontrada";
        //            retorno.MensagemUsuario = "Empresa não encontrada";
        //        }
        //        else
        //        {

        //            var listaEmpresa_Servico = new List<EmpresaPrestadoraServicoSubtipo>() {  };
        //            // crio e faço a relação da empresa prestadora e serviços e subserviços por meio da tabela / entidade  EmpresaPrestadoraServicoSubtipo
        //            foreach (var prestadorServicoSubtipo in listaEmpresaPrestadoraServicoSubtipoDTO)
        //            {
        //                var servico = await _servicoRepository.BuscarServico(prestadorServicoSubtipo.ServicoId);

        //                if (servico?.ServicoSubtipos?.Any(x => x.Id == prestadorServicoSubtipo.ServicoSubtipoId) != true)
        //                    continue;

        //                var subTipo = servico.ServicoSubtipos.FirstOrDefault(x => x.Id == prestadorServicoSubtipo.ServicoSubtipoId);

        //                if (subTipo == null)
        //                    continue;

        //                var empresaPrestadoraServicoSubtipo = new EmpresaPrestadoraServicoSubtipo
        //                {
        //                    EmpresaPrestadora = empresa,
        //                    EmpresaPrestadoraId = empresaId,
        //                    Servico = servico,
        //                    ServicoId = servico.Id,
        //                    ServicoSubtipoId = prestadorServicoSubtipo.ServicoSubtipoId,
        //                    ServicoSubtipo = subTipo
        //                };

        //                listaEmpresa_Servico.Add(empresaPrestadoraServicoSubtipo);
        //            }

        //            // adiciono no banco essas informações

        //            var resultado =  await _servicoRepository.AdicionarServicoAEmpresa(empresaId, listaEmpresa_Servico);

        //            retorno.Sucesso = resultado;
        //            retorno.HttpStatusCode = resultado ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
        //            retorno.MensagemSistema = resultado ? "Serviço cadastrado para a empresa em questão" : "Não foi possivel cadastrar  serviços para a empresa em questão";
        //            retorno.MensagemUsuario = resultado ? "Serviço cadastrado para a empresa em questão" : "Não foi possivel cadastrar  serviços para a empresa em questão";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        retorno.Sucesso = false;
        //        retorno.HttpStatusCode = HttpStatusCode.InternalServerError;
        //        retorno.MensagemSistema = ex.ToString();
        //        retorno.MensagemUsuario = "Não foi possível criar um condomínio";
        //        retorno.Dados = null;
        //    }

        //    return retorno;
        //}

        public async Task<RetornoGenerico> VincularServicoAEmpresa(Guid empresaId, List<EmpresaPrestadoraServicoSubtipoDTO> listaEmpresaPrestadoraServicoSubtipoDTO)
        {
            var retorno = new RetornoGenerico();
            try
            {
                // Buscar a empresa prestadora
                var empresa = await _empresaPrestadoraRepository.BuscarEmpresaCompleto(empresaId);

                if (empresa is null)
                {
                    retorno.Sucesso = false;
                    retorno.HttpStatusCode = HttpStatusCode.NotFound;
                    retorno.MensagemSistema = "Empresa não encontrada";
                    retorno.MensagemUsuario = "Empresa não encontrada";
                }
                else
                {
                    var listaEmpresa_Servico = new List<EmpresaPrestadoraServicoSubtipo>();

                    foreach (var prestadorServicoSubtipo in listaEmpresaPrestadoraServicoSubtipoDTO)
                    {
                        var servico = await _servicoRepository.BuscarServico(prestadorServicoSubtipo.ServicoId);

                        // Verificar se o serviço e subtipo existem
                        if (servico?.ServicoSubtipos?.Any(x => x.Id == prestadorServicoSubtipo.ServicoSubtipoId) != true)
                            continue;

                        var subTipo = servico.ServicoSubtipos.FirstOrDefault(x => x.Id == prestadorServicoSubtipo.ServicoSubtipoId);

                        if (subTipo == null)
                            continue;

                        var empresaPrestadoraServicoSubtipo = new EmpresaPrestadoraServicoSubtipo
                        {
                            EmpresaPrestadora = null,
                            EmpresaPrestadoraId = empresaId,
                            Servico = null,
                            ServicoId = servico.Id,
                            ServicoSubtipoId = prestadorServicoSubtipo.ServicoSubtipoId,
                            ServicoSubtipo = null,
                        };

                        listaEmpresa_Servico.Add(empresaPrestadoraServicoSubtipo);
                    }

                    // Adicionar os novos serviços à empresa
                    var resultado = await _servicoRepository.AdicionarServicoAEmpresa(empresaId, listaEmpresa_Servico);

                    retorno.Sucesso = resultado;
                    retorno.HttpStatusCode = resultado ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
                    retorno.MensagemSistema = resultado ? "Serviço cadastrado para a empresa em questão" : "Não foi possível cadastrar serviços para a empresa em questão";
                    retorno.MensagemUsuario = resultado ? "Serviço cadastrado para a empresa em questão" : "Não foi possível cadastrar serviços para a empresa em questão";
                }
            }
            catch (Exception ex)
            {
                retorno.Sucesso = false;
                retorno.HttpStatusCode = HttpStatusCode.InternalServerError;
                retorno.MensagemSistema = ex.ToString();
                retorno.MensagemUsuario = "Não foi possível criar o vínculo de serviços";
            }

            return retorno;
        }

    }
}
