namespace Ucondo_Financeiro_Dominio.Entities
{
    public class EntitieBase
    {
        public Guid Id { get; set; }
        public DateTime InsertDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
    }
}
