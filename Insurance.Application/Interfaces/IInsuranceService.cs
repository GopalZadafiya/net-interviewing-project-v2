using Insurance.Application.Dto;
using System.Threading.Tasks;

namespace Insurance.Application.Interfaces
{
    public interface IInsuranceService
    {
        Task<ProductInsuranceResponse> GetInsuranceAsync(int productId);

        Task<OrderInsuranceResponse> GetInsuranceAsync(int[] productIds);
    }
}
