using Microsoft.EntityFrameworkCore;
using Ucondo_Financeiro_Dominio.Entities;

namespace Ucondo_Financeiro_Data
{
    public class FinanceiroDbContext : DbContext
    {
        private static string _connectionString = "Data Source=NT-04747\\SQLEXPRESS;Initial Catalog=Ucondo-Financeiro;Integrated Security=True";
        public FinanceiroDbContext() : base()
        {
        }

        public DbSet<ContaRateio> ContasRateio { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(_connectionString);
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }
}
