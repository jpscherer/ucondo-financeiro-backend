using AutoMapper;
using Ucondo_Financeiro_Api.Contratos;
using Ucondo_Financeiro_Dominio.DTO;
using Ucondo_Financeiro_Dominio.Entities;

namespace Ucondo_Financeiro_Api.Mappers
{
    public class ApplicationProfile : Profile
    {
        public ApplicationProfile()
        {
            ContratosEEntidades();
            ContratosEDTOs();
        }
        private void ContratosEEntidades()
        {
            CreateMap<ContaRateio, ContaRateioContrato>().ReverseMap();
        }
        private void ContratosEDTOs()
        {
            CreateMap<ProximoCodigoContaRateioDTO, ProximoCodigoContaRateioContrato>().ReverseMap();
        }
    }
}
