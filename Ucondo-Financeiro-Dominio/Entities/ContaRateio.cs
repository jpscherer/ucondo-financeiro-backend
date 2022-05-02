using System.ComponentModel.DataAnnotations;

namespace Ucondo_Financeiro_Dominio.Entities
{
    public class ContaRateio : EntitieBase
    {
        [Required]
        public int Codigo { get; set; }
        [MaxLength(50), Required]
        public string Nome { get; set; }
        [Required]
        public bool AceitaLancamentos { get; set; }
        [Required]
        public int Tipo { get; set; }
        public Guid? ContaPaiId { get; set; }
    }
}
