using Insurance.Application.Dto;
using Insurance.Application.Interfaces;
using Insurance.Domain.Entities;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Insurance.Application.Services
{
    public class SurchargeRateService : ISurchargeRateService
    {
        private readonly IProductService _productService;
        private readonly ISurchargeRateRepository _surchargeRateRepository;
        private readonly ILogger<SurchargeRateService> _logger;

        public SurchargeRateService(IProductService productService,
            ISurchargeRateRepository surchargeRateRepository,
            ILogger<SurchargeRateService> logger)
        {
            _productService = productService;
            _surchargeRateRepository = surchargeRateRepository;
            _logger = logger;
        }

        public async Task<SurchargeRateDto> FindByProductTypeAsync(int productTypeId)
        {
            var entity = await _surchargeRateRepository.FindByProductTypeAsync(productTypeId);

            return new SurchargeRateDto { Value = entity.Value, ProductTypeId = entity.ProductTypeId };
        }

        public async Task<SurchargeRate> CreateAsync(SurchargeRateDto model)
        {
            var productType = await _productService.GetProductTypeAsync(model.ProductTypeId);
            if (productType == null)
            {
                _logger.LogError($"No product found for {model.ProductTypeId}");
                return null;
            }

            var entity = new SurchargeRate
            {
                ProductTypeId = model.ProductTypeId,
                Value = model.Value
            };

            await _surchargeRateRepository.AddAsync(entity);

            return entity;
        }
    }
}
