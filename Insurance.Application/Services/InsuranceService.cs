﻿using Insurance.Application.Dto;
using Insurance.Application.Helper;
using Insurance.Application.Interfaces;
using Insurance.Domain.Enums;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace Insurance.Application.Services
{
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

        public async Task<ProductInsuranceResponse> GetInsuranceAsync(int productId)
        {
            float totalInsurance = 0;

            var product = await _productService.GetProductAsync(productId);
            if (product == null)
            {
                _logger.LogInformation($"No product found for {productId}");
                return new ProductInsuranceResponse();
            }

            var productType = await _productService.GetProductTypeAsync(product.ProductTypeId);
            if (productType == null)
            {
                _logger.LogInformation($"No product type found for {product.ProductTypeId}");
                return new ProductInsuranceResponse();
            }

            if (productType.CanBeInsured) //Should be insured only if product is eligible 
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

        public async Task<OrderInsuranceResponse> GetInsuranceAsync(int[] productIds)
        {
            float totalInsurance = 0;

            var insuranceRequest = new OrderInsuranceRequest();

            foreach (var productId in productIds)
            {
                //Retrieve base insurance
                var productInsurance = await GetInsuranceAsync(productId);
                totalInsurance += productInsurance.InsuranceValue;

                //Populate list of products insurance to check product type
                insuranceRequest.ProductInsurances.Add(productInsurance);
            }

            if (DoesOrderContainDigitalCamera(insuranceRequest))
            {
                totalInsurance += DIGITAL_CAMERA_INSURANCE; //Additional insurance for digital camera
            }

            return new OrderInsuranceResponse { TotalInsurance = totalInsurance };
        }

        private static bool DoesOrderContainDigitalCamera(OrderInsuranceRequest orderDto)
        {
            return orderDto.ProductInsurances
                .Any(x => x.ProductTypeId.Equals((int)ProductTypes.DigitalCameras));
        }
    }
}