using Insurance.Application.Dto;
using Insurance.Application.Exceptions;
using Insurance.Application.Helper;
using Insurance.Application.Interfaces;
using Insurance.Domain.Enums;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace Insurance.Application.Services
{
    /// <summary>
    /// Implementation of Insurance calculation business logic
    /// </summary>
    public class InsuranceService : IInsuranceService
    {
        private readonly IProductService _productService;
        private readonly ISurchargeRateService _surchargeRateService;
        private readonly ILogger<InsuranceService> _logger;

        private const float DIGITAL_CAMERA_INSURANCE = 500;

        public InsuranceService(IProductService productService,
            ISurchargeRateService surchargeRateService,
            ILogger<InsuranceService> logger)
        {
            _productService = productService;
            _surchargeRateService = surchargeRateService;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<ProductInsuranceResponse> GetInsuranceByProductAsync(int productId)
        {
            float totalInsurance = 0;

            var product = await _productService.GetProductAsync(productId);
            if (product == null)
            {
                throw new ProductNotFoundException($"No product found for product id: {productId}");
            }

            var productType = await _productService.GetProductTypeAsync(product.ProductTypeId);
            if (productType == null)
            {
                throw new ProductTypeNotFoundException($"No product type found for product type id: {product.ProductTypeId}");
            }

            //Should be insured only if product is eligible
            if (productType.CanBeInsured)
            {
                totalInsurance += InsuranceCalculator.GetBySalesPrice(product.SalesPrice); //Based on price
                totalInsurance += InsuranceCalculator.GetByProductType(product.ProductTypeId); //Based on product type

                //Additional surcharge
                var surcharge = await _surchargeRateService.FindByProductTypeAsync(product.ProductTypeId);
                if (surcharge != null)
                {
                    totalInsurance += surcharge.Value;
                }
            }

            return new ProductInsuranceResponse
            {
                InsuranceValue = totalInsurance,
                ProductId = productId,
                ProductTypeId = product.ProductTypeId
            };
        }

        /// <inheritdoc />
        public async Task<OrderInsuranceResponse> GetInsuranceByOrderAsync(int[] productIds)
        {
            if (productIds == null || productIds.Length == 0)
            {
                _logger.LogWarning($"No valid products in your order");
                throw new BadRequestException($"No valid products in your order");
            }

            float totalInsurance = 0;

            var insuranceRequest = new OrderInsuranceRequest();

            foreach (var productId in productIds)
            {
                //Retrieve base insurance
                var productInsurance = await GetInsuranceByProductAsync(productId);
                totalInsurance += productInsurance.InsuranceValue;

                //Populate list of products insurance to check product type
                insuranceRequest.ProductInsurances.Add(productInsurance);
            }

            if (DoesOrderContainDigitalCamera(insuranceRequest))
            {
                totalInsurance += DIGITAL_CAMERA_INSURANCE;
            }

            return new OrderInsuranceResponse { TotalInsurance = totalInsurance };
        }

        /// <summary>
        /// Check if order contains any digital camera 
        /// </summary>
        /// <param name="orderDto"></param>
        /// <returns></returns>
        private static bool DoesOrderContainDigitalCamera(OrderInsuranceRequest orderDto)
        {
            return orderDto.ProductInsurances
                .Any(x => x.ProductTypeId.Equals((int)ProductTypes.DigitalCameras));
        }
    }
}
