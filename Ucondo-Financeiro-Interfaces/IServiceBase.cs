namespace Ucondo_Financeiro_Interfaces
{
    public interface IServiceBase<E> where E : IEntitieBase
    {
        public void Inserir(E entidade);
        public bool Deletar(Guid id);
        public IEnumerable<E> Listar();
    }
}
