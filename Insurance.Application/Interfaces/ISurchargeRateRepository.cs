using Insurance.Domain.Entities;
using System.Threading.Tasks;

namespace Insurance.Application.Interfaces
{
    public interface ISurchargeRateRepository
    {
        /// <summary>
        /// Get surcharge rate by product type
        /// </summary>
        /// <param name="productTypeId"> Product type id </param>
        /// <returns></returns>
        Task<SurchargeRate> FindByProductTypeAsync(int productTypeId);

        /// <summary>
        /// Add surcharge rate for product type
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<SurchargeRate> CreateAsync(SurchargeRate entity);
    }
}
