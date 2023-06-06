using Insurance.Application.Helper;
using Insurance.Domain.Enums;
using Xunit;

namespace Insurance.Tests.Functional
{
    public class InsuranceCalculatorTests
    {
        [Theory]
        
        [InlineData(499, (int)ProductTypes.Laptops, 500)]
        [InlineData(500, (int)ProductTypes.Laptops, 1500)]
        [InlineData(1999, (int)ProductTypes.Laptops, 1500)]
        [InlineData(2000, (int)ProductTypes.Laptops, 2500)]

        [InlineData(499, (int)ProductTypes.WashingMachines, 0)]
        [InlineData(500, (int)ProductTypes.WashingMachines, 1000)]
        [InlineData(1999, (int)ProductTypes.WashingMachines, 1000)]
        [InlineData(2000, (int)ProductTypes.WashingMachines, 2000)]

        public void VerifyInsuranceCalculation(float productPrice, int productTypeId, int expectedInsurance)
        {
            var basicInsurance = InsuranceCalculator.GetBySalesPrice(productPrice);
            var additionalInsurance = InsuranceCalculator.GetByProductType(productTypeId);

            var totalInsurance = basicInsurance + additionalInsurance;

            Assert.Equal(expectedInsurance, totalInsurance);
        }
    }
}
