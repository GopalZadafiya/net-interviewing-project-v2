using Insurance.Application.Interfaces;
using Insurance.Domain.Entities;
using Insurance.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Insurance.Infrastructure.Repositories
{
    public class SurchargeRateRepository : ISurchargeRateRepository
    {
        private readonly InsuranceDbContext _dbContext;
        private readonly ILogger<SurchargeRateRepository> _logger;

        public SurchargeRateRepository(InsuranceDbContext dbContext, ILogger<SurchargeRateRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<SurchargeRate> FindByProductTypeAsync(int productTypeId)
        {
            return await _dbContext.SurchargeRates
                .FirstOrDefaultAsync(s => s.ProductTypeId == productTypeId);
        }

        public async Task<SurchargeRate> AddAsync(SurchargeRate entity)
        {
            try
            {                
                await _dbContext.AddAsync(entity);
                await _dbContext.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to add surcharge rate - {ex.Message}\n{ex.StackTrace}");
                return null;
            }
        }
    }
}
