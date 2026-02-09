using Gerenciado_de_Usuario_Papido_Facil.Domain.Entities;

namespace Gerenciado_de_Usuario_Papido_Facil.Infra.Data.Interfaces
{
    public interface IAnaliseIdoneidadeRepository
    {
        Task<Idoneidade?> BuscarIdoneidadeAsync(Guid idoneidadeId);
        Task<Idoneidade> CadastrarIdoneidadeAsync(Idoneidade idoneidade);
        Task EditarIdoneidadeAsync(Idoneidade idoneidade);
        Task<CND> CadastrarCND(CND certidao);
        Task<CNDT> CadastrarCNDT(CNDT certidao);
    }
}
