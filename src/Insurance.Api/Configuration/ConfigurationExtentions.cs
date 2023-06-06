using Insurance.Infrastructure.Persistance;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Insurance.Api.Configuration
{
    public static class ConfigurationExtentions
    {
        public static void AddDatabaseConfiguration(this IServiceCollection services)
        {
            var something = services.BuildServiceProvider();
            var environment = something.GetRequiredService<IWebHostEnvironment>();

            if (environment.IsDevelopment()) 
            {
                services.AddDbContext<InsuranceDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InsuranceDb");
                });
            }
            else
            {
                // configure your database provider
            }
        }
    }
}
