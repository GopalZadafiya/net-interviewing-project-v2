using Insurance.Application.Dto;
using Insurance.Application.Exceptions;
using Insurance.Application.Interfaces;
using Insurance.Application.Services;
using Insurance.Domain.Entities;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Insurance.Tests.Services
{
    public class SurchargeRateServiceTests
    {
        private readonly Mock<IProductService> _productServiceMock;
        private readonly Mock<ISurchargeRateRepository> _repositoryMock;
        private readonly SurchargeRateService _sut;

        public SurchargeRateServiceTests()
        {
            _productServiceMock = new Mock<IProductService>();
            _repositoryMock = new Mock<ISurchargeRateRepository>();

            _sut = new SurchargeRateService(_productServiceMock.Object, _repositoryMock.Object);
        }

        [Fact]
        public async Task FindByProductTypeAsync_Should_Throw_WhenProductTypeDoesNotExists()
        {
            //Arrange
            _productServiceMock
               .Setup(s => s.GetProductTypeAsync(It.IsAny<int>()));

            //Act & Assert
            await Assert.ThrowsAsync<ProductTypeNotFoundException>(async ()
                => await _sut.FindByProductTypeAsync(It.IsAny<int>()));
        }

        [Fact]
        public async Task FindByProductTypeAsync_Should_Return_NotFound_WhenProductTypeExistsButSurchargeNot()
        {
            //Arrange
            _productServiceMock
               .Setup(s => s.GetProductTypeAsync(It.IsAny<int>()))
               .ReturnsAsync(SetupProductType(365, true));

            _repositoryMock
                .Setup(r => r.FindByProductTypeAsync(It.IsAny<int>()));

            //Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(async ()
                => await _sut.FindByProductTypeAsync(It.IsAny<int>()));
        }

        [Fact]
        public async Task FindByProductTypeAsync_Should_Return_SurchargeRate_WhenProductAndRateExists()
        {
            //Arrange
            float expectedSurchargeRate = 385;
            var productType = SetupProductType(365, true);

            _productServiceMock
               .Setup(s => s.GetProductTypeAsync(It.IsAny<int>()))
               .ReturnsAsync(productType);

            _repositoryMock
                .Setup(r => r.FindByProductTypeAsync(It.IsAny<int>()))
                .ReturnsAsync(SetupSurchargeRate(productType.Id, expectedSurchargeRate));

            //Act
            var result = await _sut.FindByProductTypeAsync(It.IsAny<int>());

            //Assert
            Assert.Equal(expectedSurchargeRate, result.Value);
        }

        [Fact]
        public async Task CreateAsync_Should_Throw_WhenProductTypeDoesNotExists()
        {
            //Arrange
            _productServiceMock
               .Setup(s => s.GetProductTypeAsync(It.IsAny<int>()));

            //Act & Assert
            await Assert.ThrowsAsync<ProductTypeNotFoundException>(async ()
                => await _sut.CreateAsync(SetupSurchargeRateDto(32, 200)));
        }

        [Fact]
        public async Task CreateAsync_Should_Create_WhenProductTypeExists()
        {
            //Arrange
            var productType = SetupProductType(32);
            var surchargeRateDto = SetupSurchargeRateDto(32, 200);
            var surchargeRate = SetupSurchargeRate(32, 200);

            _productServiceMock
               .Setup(s => s.GetProductTypeAsync(It.IsAny<int>()))
               .ReturnsAsync(productType);
            _repositoryMock
                .Setup(r => r.CreateAsync(It.IsAny<SurchargeRate>()))
                .ReturnsAsync(surchargeRate);

            //Act 
            var result = await _sut.CreateAsync(surchargeRateDto);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(result.Value, surchargeRateDto.Value);

        }

        #region Private setup methods
        private ProductType SetupProductType(int id, bool canBeInsured = true)
        {
            return new ProductType
            {
                Id = id,
                Name = "Test",
                CanBeInsured = canBeInsured
            };
        }

        private SurchargeRate SetupSurchargeRate(int id, float value)
        {
            return new SurchargeRate
            {
                ProductTypeId = id,
                Value = value
            };
        }

        private SurchargeRateDto SetupSurchargeRateDto(int id, float value)
        {
            return new SurchargeRateDto
            {
                ProductTypeId = id,
                Value = value
            };
        }

        #endregion Private setup methods
    }
}
