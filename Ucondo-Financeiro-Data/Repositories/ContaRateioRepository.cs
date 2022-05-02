using Microsoft.EntityFrameworkCore;
using Ucondo_Financeiro_Dominio.Entities;
using Ucondo_Financeiro_Dominio.Interfaces.Repositories;

namespace Ucondo_Financeiro_Data.Repositories
{
    public class ContaRateioRepository : IContaRateioRepository
    {
        private readonly FinanceiroDbContext _dbContext;

        public ContaRateioRepository(FinanceiroDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ContaRateio Alterar(ContaRateio contaRateio)
        {
            contaRateio.UpdateDate = DateTime.Now;
            _dbContext.ContasRateio.Update(contaRateio);
            _dbContext.SaveChanges();

            return contaRateio;
        }

        public void Deletar(Guid id)
        {
            var contaRateio = GetAll().FirstOrDefault(s => s.Id == id);
            if (contaRateio.Id != Guid.Empty)
            {
                contaRateio.DeleteDate = DateTime.Now;
                _dbContext.ContasRateio.Update(contaRateio);
                _dbContext.SaveChanges();
            }
        }

        public IEnumerable<ContaRateio> GetAll()
        {
            return _dbContext.ContasRateio.Where(w => w.DeleteDate == null).AsNoTracking();
        }

        public ContaRateio Inserir(ContaRateio contaRateio)
        {
            contaRateio.InsertDate = DateTime.Now;
            _dbContext.ContasRateio.Add(contaRateio);
            _dbContext.SaveChanges();

            return contaRateio;
        }
    }
}
