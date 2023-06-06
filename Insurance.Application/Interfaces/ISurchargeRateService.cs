using Insurance.Application.Dto;
using Insurance.Domain.Entities;
using System.Threading.Tasks;

namespace Insurance.Application.Interfaces
{
    public interface ISurchargeRateService
    {
        Task<SurchargeRateDto> FindByProductTypeAsync(int productTypeId);

        Task<SurchargeRate> CreateAsync(SurchargeRateDto model);
    }
}
