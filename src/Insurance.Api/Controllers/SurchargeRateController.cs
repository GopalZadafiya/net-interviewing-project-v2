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

        [HttpGet]
        public async Task<IActionResult> GetAsync(int id)
        {
            var result = await _surchargeRateService.FindByProductTypeAsync(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddSurchargeRateAsync(SurchargeRateDto model)
        {
            var result = await _surchargeRateService.CreateAsync(model);
            return Ok(result);
        }
    }
}
