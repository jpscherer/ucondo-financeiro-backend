namespace Ucondo_Financeiro_Interfaces
{
    public interface IEntitieBase
    {
        Guid Id { get; set; }
        DateTime InsertDate { get; set; }
        DateTime? UpdateDate { get; set; }
        DateTime? DeleteDate { get; set; }
    }
}
