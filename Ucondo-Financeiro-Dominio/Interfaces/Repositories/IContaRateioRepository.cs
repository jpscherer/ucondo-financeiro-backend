using Ucondo_Financeiro_Dominio.Entities;

namespace Ucondo_Financeiro_Dominio.Interfaces.Repositories
{
    public interface IContaRateioRepository
    {
        public IEnumerable<ContaRateio> GetAll();
        public void Deletar(Guid id);
        public ContaRateio Inserir(ContaRateio contaRateio);
        public ContaRateio Alterar(ContaRateio contaRateio);
    }
}
