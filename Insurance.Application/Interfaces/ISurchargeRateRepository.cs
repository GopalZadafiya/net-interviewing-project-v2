using Insurance.Domain.Entities;
using System.Threading.Tasks;

namespace Insurance.Application.Interfaces
{
    public interface ISurchargeRateRepository
    {
        Task<SurchargeRate> FindByProductTypeAsync(int productTypeId);

        Task<SurchargeRate> AddAsync(SurchargeRate entity);
    }
}
