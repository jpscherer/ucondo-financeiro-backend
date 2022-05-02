using Ucondo_Financeiro_Data;
using Ucondo_Financeiro_Data.Repositories;
using Ucondo_Financeiro_Dominio.Interfaces.Repositories;
using Ucondo_Financeiro_Dominio.Interfaces.Services;
using Ucondo_Financeiro_Service.Services;

namespace Ucondo_Financeiro_Api
{
    public class DependencyInjectionManager
    {
        public static void InjectAppDependecies(IServiceCollection services)
        {
            InjetarServices(services);
            InjetarRepositorios(services);
            InjetarContextoBanco(services);
        }
        public static void InjetarServices(IServiceCollection services)
        {
            services.AddScoped<IContaRateioService, ContaRateioService>();
        }
        public static void InjetarRepositorios(IServiceCollection services)
        {
            services.AddScoped<IContaRateioRepository, ContaRateioRepository>();
        }
        public static void InjetarContextoBanco(IServiceCollection services)
        {
            services.AddScoped<FinanceiroDbContext>();
        }
    }
}
