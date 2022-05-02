namespace Ucondo_Financeiro_Api.Contratos
{
    public class ContaRateioContrato
    {
        public Guid Id { get; set; }
        public int Codigo { get; set; }
        public string Nome { get; set; }
        public bool AceitaLancamentos { get; set; }
        public int Tipo { get; set; }
        public Guid ContaPaiId { get; set; }
    }
}
