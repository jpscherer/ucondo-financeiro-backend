namespace Ucondo_Financeiro_Dominio.DTO
{
    public class ProximoCodigoContaRateioDTO
    {
        public Guid IdContaPai { get; set; }
        public int Codigo { get; set; }
        public bool ContaPaiAlterada { get; set; }
    }
}
