using Insurance.Application.Interfaces;
using Insurance.Domain.Entities;
using Insurance.Infrastructure.Services.ApiClient;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Insurance.Infrastructure.Services
{
    public class ProductService : IProductService
    {
        private readonly IApiClient _apiClient;
        private readonly ILogger<ProductService> _logger;

        private const string HOST_URI = "http://localhost:5002";

        public ProductService(IApiClient apiClient, ILogger<ProductService> logger)
        {
            _apiClient = apiClient;
            _logger = logger;
        }

        public async Task<Product> GetProductAsync(int productId)
        {
            _logger.LogInformation($"Retrieving product by Id {productId}");

            var endpoint = $"{HOST_URI}/products/{productId}";

            return await _apiClient.GetAsync<Product>(endpoint);
        }

        public async Task<ProductType> GetProductTypeAsync(int productTypeId)
        {
            _logger.LogInformation($"Retrieving product type by Id {productTypeId}");

            var endpoint = $"{HOST_URI}/product_types/{productTypeId}";

            return await _apiClient.GetAsync<ProductType>(endpoint);
        }

        public async Task<List<ProductType>> GetAllProductTypesAsync()
        {
            _logger.LogInformation($"Retrieving all product types");

            var endpoint = $"{HOST_URI}/product_types";

            return await _apiClient.GetAsync<List<ProductType>>(endpoint);
        }
    }
}
