using Ucondo_Financeiro_Dominio.DTO;
using Ucondo_Financeiro_Dominio.Entities;

namespace Ucondo_Financeiro_Dominio.Interfaces.Services
{
    public interface IContaRateioService
    {
        void Inserir(ContaRateio conta);
        void Alterar(ContaRateio conta);
        void Deletar(Guid id);
        IEnumerable<ContaRateio> ListarContasRaiz();
        IEnumerable<ContaRateio> Listar(string pesquisa);
        IEnumerable<ContaRateio> ListarByContaPai(Guid idContaPai);
        ProximoCodigoContaRateioDTO ProximoCodigo(Guid? idContaPai);
    }
}
