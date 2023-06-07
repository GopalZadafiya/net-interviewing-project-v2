using Insurance.Application.Dto;
using Insurance.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Insurance.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InsuranceController : Controller
    {
        private readonly IInsuranceService _insuranceService;

        public InsuranceController(IInsuranceService insuranceService)
        {
            _insuranceService = insuranceService;
        }

        /// <summary>
        /// Get product insurance by id 
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpGet("byProduct/{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductInsuranceResponse))]
        public async Task<IActionResult> GetProductInsurance(int productId)
        {
            var result = await _insuranceService.GetInsuranceByProductAsync(productId);

            return Ok(result);
        }

        /// <summary>
        /// Get insurance of all products in your order or cart
        /// </summary>
        /// <param name="productIds"></param>
        /// <returns></returns>
        [HttpPost("byOrder")]
        public async Task<IActionResult> GetOrderInsurance([FromBody] int[] productIds)
        {
            var result = await _insuranceService.GetInsuranceByOrderAsync(productIds);
            
            return Ok(result);
        }
    }
}