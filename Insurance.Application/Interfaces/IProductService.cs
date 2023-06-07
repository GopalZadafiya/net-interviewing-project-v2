using Insurance.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Insurance.Application.Interfaces
{
    public interface IProductService
    {
        /// <summary>
        /// Get product by id
        /// </summary>
        /// <param name="id"> Product id </param>
        /// <returns></returns>
        Task<Product> GetProductAsync(int id);

        /// <summary>
        /// Get product type by id
        /// </summary>
        /// <param name="id"> Product type id </param>
        /// <returns></returns>
        Task<ProductType> GetProductTypeAsync(int id);

        /// <summary>
        /// Get all product types
        /// </summary>
        /// <returns></returns>
        Task<List<ProductType>> GetAllProductTypesAsync();
    }
}
