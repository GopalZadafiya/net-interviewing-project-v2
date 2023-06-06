using Insurance.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Insurance.Application.Interfaces
{
    public interface IProductService
    {
        Task<Product> GetProductAsync(int id);

        Task<ProductType> GetProductTypeAsync(int productType);

        Task<List<ProductType>> GetAllProductTypesAsync();
    }
}
