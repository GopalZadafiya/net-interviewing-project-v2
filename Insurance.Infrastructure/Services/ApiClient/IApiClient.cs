using System.Threading.Tasks;

namespace Insurance.Infrastructure.Services.ApiClient
{
    public interface IApiClient
    {
        Task<T> GetAsync<T>(string apiUrl);
    }
}
