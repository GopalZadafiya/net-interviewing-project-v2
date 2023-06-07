using Insurance.Application.Dto;
using System.Threading.Tasks;

namespace Insurance.Application.Interfaces
{
    public interface IInsuranceService
    {
        /// <summary>
        /// Calculate amount of insurance for a given product
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        Task<ProductInsuranceResponse> GetInsuranceByProductAsync(int productId);

        /// <summary>
        /// Calculate amount of insurance for a given order (products in cart)
        /// </summary>
        /// <param name="productIds"></param>
        /// <returns></returns>
        Task<OrderInsuranceResponse> GetInsuranceByOrderAsync(int[] productIds);
    }
}
