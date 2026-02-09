using AutoMapper;
using Gerenciado_de_Usuario_Rapido_Facil.Application.DTOs;
using Gerenciado_de_Usuario_Rapido_Facil.Application.ViewModel;
using Gerenciado_de_Usuario_Rapido_Facil.Domain.Entities;

namespace Gerenciado_de_Usuario_Rapido_Facil.Application.Configuration
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CadastroCondominioDTO, Condominio>();
            CreateMap<EdicaoCondominioDTO, Condominio>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) =>
                    srcMember != null &&
                    !(srcMember is string str && string.IsNullOrWhiteSpace(str)))); ;

            CreateMap<CadastroCondominoDTO, Condomino>()
                            .ForMember(dest => dest.CodigoVinculacao, opt => opt.MapFrom(src => src.CodigoVinculo));

            CreateMap<EdicaoCondominoDTO, Condomino>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) =>
                    srcMember != null &&
                    !(srcMember is string str && string.IsNullOrWhiteSpace(str))));

            //// Mapeamento de Empresa para EmpresaPrestadora
            //CreateMap<Empresa, EmpresaPrestadora>()
            //    .ForMember(dest => dest.Id, opt => opt.Ignore()); // Ignorar o mapeamento do Id se ele for gerado automaticamente

            // Se você tiver DTOs adicionais, configure o mapeamento aqui
            CreateMap<EmpresaPrestadoraDTO, EmpresaPrestadora>()
                .ForMember(dest => dest.CnpjCpf, opt => opt.MapFrom(src => src.CnpjCpf.Replace(".", "").Replace("/", "").Replace("-", "")))
                .ForMember(dest => dest.EmpresaPrestadoraServicoSubtipos, opt => opt.Ignore());


            CreateMap<EmpresaPrestadoraAtualizacaoDTO, EmpresaPrestadora>()
                 .ForAllMembers(opt => opt.Condition((src, dest, srcMember) =>
                     srcMember != null &&
                     !(srcMember is string str && string.IsNullOrWhiteSpace(str))));

            CreateMap<EmpresaPrestadoraServicoSubtipoDTO, EmpresaPrestadoraServicoSubtipo>();

            CreateMap<Condominio, CondominioViewModel>();

            CreateMap<Condomino, CondominoViewModel>();

            CreateMap<CriarChatDTO, Chat>();
            CreateMap<CriarMensagemChatDTO, MensagemChat>();

            // Mapeamento da Entidade para ViewModel
            CreateMap<Chat, ChatRespostaViewModel>();

            CreateMap<MensagemChat, MessageChatViewModel>();

            // Mapeamento do DTO de entrada para Entidade
            CreateMap<CriarChatDTO, Chat>();

        }
    }
}
