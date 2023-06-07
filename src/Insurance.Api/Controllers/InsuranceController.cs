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


        [HttpGet("byProduct/{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductInsuranceResponse))]
        public async Task<IActionResult> GetProductInsurance(int productId)
        {
            var result = await _insuranceService.GetInsuranceAsync(productId);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }


        [HttpPost("byOrder")]
        public async Task<IActionResult> GetOrderInsurance([FromBody] int[] productIds)
        {
            var result = await _insuranceService.GetInsuranceAsync(productIds);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
    }
}