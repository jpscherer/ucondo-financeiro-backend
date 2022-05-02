using Ucondo_Financeiro_Dominio.Entities;

namespace Ucondo_Financeiro_Testes.Builders
{
    public class ContaRateioBuilder
    {
        private ContaRateio contaRateio;
        public ContaRateioBuilder()
        {
            contaRateio = new ContaRateio();
        }
        public ContaRateio Build()
        {
            return contaRateio;
        }
        public ContaRateioBuilder ComId(Guid id)
        {
            contaRateio.Id = id;
            return this;
        }
        public ContaRateioBuilder ComCodigo(int codigo)
        {
            contaRateio.Codigo = codigo;
            return this;
        }
        public ContaRateioBuilder ComNome(string nome)
        {
            contaRateio.Nome = nome;
            return this;
        }
        public ContaRateioBuilder ComContaPai(Guid idContaPai)
        {
            contaRateio.ContaPaiId = idContaPai;
            return this;
        }
        public ContaRateioBuilder ComTipo(int tipo)
        {
            contaRateio.Tipo = tipo;
            return this;
        }

        public ContaRateioBuilder ComLancamento(bool aceitaLancamento)
        {
            contaRateio.AceitaLancamentos = aceitaLancamento;
            return this;
        }
    }
}
