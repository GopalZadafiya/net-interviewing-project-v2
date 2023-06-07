using Insurance.Application.Dto;
using Insurance.Application.Exceptions;
using Insurance.Application.Interfaces;
using Insurance.Domain.Entities;
using System.Threading.Tasks;

namespace Insurance.Application.Services
{
    /// <summary>
    /// Implementation of surcharge business logic
    /// </summary>
    public class SurchargeRateService : ISurchargeRateService
    {
        private readonly IProductService _productService;
        private readonly ISurchargeRateRepository _surchargeRateRepository;

        public SurchargeRateService(IProductService productService,
            ISurchargeRateRepository surchargeRateRepository)
        {
            _productService = productService;
            _surchargeRateRepository = surchargeRateRepository;
        }

        /// <inheritdoc />
        public async Task<SurchargeRateDto> FindByProductTypeAsync(int productTypeId)
        {
            var productType = await _productService.GetProductTypeAsync(productTypeId);
            if (productType == null)
            {
                throw new ProductTypeNotFoundException($"Product type {productTypeId} does not exist.");
            }

            var entity = await _surchargeRateRepository.FindByProductTypeAsync(productTypeId);
            if (entity == null)
            {
                throw new NotFoundException($"Surcharge rate does not exist for product type {productTypeId}.");
            }

            return new SurchargeRateDto { Value = entity.Value, ProductTypeId = entity.ProductTypeId };
        }

        /// <inheritdoc />
        public async Task<SurchargeRate> CreateAsync(SurchargeRateDto model)
        {
            var productType = await _productService.GetProductTypeAsync(model.ProductTypeId);
            if (productType == null)
            {
                throw new ProductTypeNotFoundException($"Product type {model.ProductTypeId} does not exist.");
            }

            var entity = new SurchargeRate
            {
                ProductTypeId = model.ProductTypeId,
                Value = model.Value
            };

            return await _surchargeRateRepository.CreateAsync(entity);
        }
    }
}
