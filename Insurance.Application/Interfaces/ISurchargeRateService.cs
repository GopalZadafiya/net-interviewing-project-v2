using Insurance.Application.Dto;
using Insurance.Domain.Entities;
using System.Threading.Tasks;

namespace Insurance.Application.Interfaces
{
    public interface ISurchargeRateService
    {
        /// <summary>
        /// Get surcharge rate by product type
        /// </summary>
        /// <param name="productTypeId"></param>
        /// <returns></returns>
        Task<SurchargeRateDto> FindByProductTypeAsync(int productTypeId);

        /// <summary>
        /// Add surcharge rate per product type
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<SurchargeRate> CreateAsync(SurchargeRateDto model);
    }
}
