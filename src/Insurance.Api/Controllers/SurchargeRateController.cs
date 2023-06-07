using Insurance.Application.Dto;
using Insurance.Application.Exceptions;
using Insurance.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Insurance.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SurchargeRateController: ControllerBase
    {
        private readonly ISurchargeRateService _surchargeRateService;

        public SurchargeRateController(ISurchargeRateService surchargeRateService)
        {
            _surchargeRateService = surchargeRateService;
        }

        /// <summary>
        /// Get surcharge rate per product
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAsync(int id)
        {
            var result = await _surchargeRateService.FindByProductTypeAsync(id);
            return Ok(result);
        }

        /// <summary>
        /// Create surcharge rate per product
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] SurchargeRateDto model)
        {
            if (model == null)
            {
                throw new BadRequestException($"{nameof(model)} is required");
            }

            var result = await _surchargeRateService.CreateAsync(model);
            return Ok(result);
        }
    }
}
