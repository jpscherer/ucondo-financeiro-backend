
namespace Ucondo_Financeiro_Interfaces
{
    public interface IContaRateioService : IServiceBase<IEntitieBase>
    {
        public void ProximoCodigoConta(Guid? idContaPai);
    }
}
