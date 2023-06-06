using Insurance.Application.Dto;
using Insurance.Application.Interfaces;
using Insurance.Application.Services;
using Insurance.Domain.Entities;
using Insurance.Domain.Enums;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Insurance.Tests.Services
{
    public class InsuranceServiceTests
    {
        private readonly Mock<IProductService> _productServiceMock;
        private readonly Mock<ISurchargeRateService> _surchargeRateServiceMock;
        private readonly Mock<ILogger<InsuranceService>> _loggerMock;

        private readonly InsuranceService _sut;

        public InsuranceServiceTests()
        {
            _productServiceMock = new Mock<IProductService>();
            _surchargeRateServiceMock = new Mock<ISurchargeRateService>();
            _loggerMock = new Mock<ILogger<InsuranceService>>();

            _sut = new InsuranceService(
                _productServiceMock.Object,
                _surchargeRateServiceMock.Object,
                _loggerMock.Object);
        }


        [Fact]
        public async Task GetInsuranceAsync_ShouldReturn_WhenProductIsNotFound()
        {
            //Arrange
            int productId = 598882;
            _productServiceMock
                .Setup(x => x.GetProductAsync(It.IsAny<int>()));

            //Act
            var result = await _sut.GetInsuranceAsync(productId);

            //Assert
            result.Equals(0);
        }

        [Fact]
        public async Task GetInsuranceAsync_ShouldReturn_WhenProductTypeIsNotFound()
        {
            //Arrange
            int productId = 598882;
            _productServiceMock
                .Setup(x => x.GetProductAsync(It.IsAny<int>()))
                .ReturnsAsync(GetProduct(productId));

            _productServiceMock
                .Setup(x => x.GetProductTypeAsync(It.IsAny<int>()));
            //Act
            var result = await _sut.GetInsuranceAsync(productId);

            //Assert
            result.Equals(0);
        }

        [Fact]
        public async Task GetInsuranceAsync_WithInvalidProductId_ShouldReturn_InsuranceAmountZero()
        {
            //Arrange
            int productId = 598882;

            _productServiceMock
                .Setup(x => x.GetProductAsync(It.IsAny<int>()))
                .ReturnsAsync(GetProduct(productId));

            _productServiceMock
                .Setup(x => x.GetProductTypeAsync(It.IsAny<int>()))
                .ReturnsAsync(GetProductType(productId, false));

            //Act
            var result = await _sut.GetInsuranceAsync(productId);

            //Assert
            result.Equals(0);
        }


        [Theory]
        [InlineData(0, (int)ProductTypes.Laptops, 0, 500)]
        [InlineData(246, (int)ProductTypes.Laptops, 123, 623)]
        [InlineData(246, (int)ProductTypes.Laptops, 0, 500)]
        [InlineData(732, (int)ProductTypes.Laptops, 123, 1623)]

        [InlineData(0, (int)ProductTypes.WashingMachines, 0, 0)]
        [InlineData(499, (int)ProductTypes.WashingMachines, 120, 120)]
        [InlineData(500, (int)ProductTypes.WashingMachines, 340, 1340)]
        [InlineData(2000, (int)ProductTypes.WashingMachines, 250, 2250)]

        [InlineData(2000, (int)ProductTypes.Laptops, 250, 0, false)]
        public async Task GetInsuranceAsync_ShouldCalculateInsurance(int salesPrice,
            int productTypeId,
            int surchargeRate,
            float expectedInsurance,
            bool canBeInsured = true)
        {
            //Arrange
            SetUpProductServices(salesPrice, productTypeId, surchargeRate, canBeInsured);

            //Act
            var result = await _sut.GetInsuranceAsync(It.IsAny<int>());

            //Assert
            Assert.Equal(expectedInsurance, result.InsuranceValue);
        }


        [Theory]
        [InlineData(320, (int)ProductTypes.WashingMachines, 340, 680)]
        [InlineData(280, (int)ProductTypes.DigitalCameras, 220, 940)]
        [InlineData(280, (int)ProductTypes.SmartPhones, 520, 2040)]
        public async Task GetOrderInsuranceAsync_ShouldCalculateInsurance(int salesPrice,
            int productTypeId,
            int surchargeRate,
            float expectedInsurance,
            bool canBeInsured = true)
        {
            //Arrange
            SetUpProductServices(salesPrice, productTypeId, surchargeRate, canBeInsured);

            //Act
            var result = await _sut.GetInsuranceAsync(new int[] { 1, 2 });

            //Assert
            Assert.Equal(expectedInsurance, result.TotalInsurance);
        }

        #region Private SetUp Methods

        private void SetUpProductServices(int salesPrice, int productTypeId, float surchargeRate, bool canBeInsured)
        {
            var product = CreateProduct(salesPrice, productTypeId);
            var productType = CreateProductType(productTypeId, canBeInsured);
            var surcharge = CreateSurchargeRate(productTypeId, surchargeRate);

            _productServiceMock
                .Setup(_ => _.GetProductAsync(It.IsAny<int>()))
                .ReturnsAsync(product);

            _productServiceMock
                .Setup(_ => _.GetProductTypeAsync(productTypeId))
                .ReturnsAsync(productType);

            _surchargeRateServiceMock
                .Setup(_ => _.FindByProductTypeAsync(It.IsAny<int>()))
                .ReturnsAsync(surcharge);
        }

        private Product GetProduct(int id)
        {
            return new Product
            {
                Id = id,
                Name = "Test",
                ProductTypeId = 45,
                SalesPrice = 503
            };
        }

        private ProductType GetProductType(int id, bool canBeInsured)
        {
            return new ProductType
            {
                Id = id,
                Name = "Test",
                CanBeInsured = canBeInsured
            };
        }

        private Product CreateProduct(int price, int productTypeId)
        {
            return new()
            {
                SalesPrice = price,
                Name = "Test Product",
                ProductTypeId = productTypeId
            };
        }

        private ProductType CreateProductType(int productTypeId, bool canBeInsured)
        {
            return new()
            {
                CanBeInsured = canBeInsured,
                Name = "Test Product",
                Id = productTypeId
            };
        }

        private SurchargeRateDto CreateSurchargeRate(int productTypeId, float surcharge)
        {
            return new()
            {
                ProductTypeId = productTypeId,
                Value = surcharge
            };
        }

        #endregion Private SetUp Methods
    }
}
