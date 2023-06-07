using FluentAssertions;
using Insurance.Api.Controllers;
using Insurance.Application.Dto;
using Insurance.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Insurance.Tests.Controllers
{
    public class SurchargeRateControllerTests
    {
        private readonly Mock<ISurchargeRateService> _surchargeRateServiceMock;
        private readonly SurchargeRateController _sut;

        public SurchargeRateControllerTests()
        {
            _surchargeRateServiceMock = new Mock<ISurchargeRateService>();

            _sut = new SurchargeRateController(_surchargeRateServiceMock.Object);
        }

        [Fact]
        public async Task GetAsync_Should_Return_SurchargeRate_WhenValidProductTypeProvided()
        {
            //Arrange
            float expectedSurchargeRate = 573; 
            var productInsurance = CreateSurchargeRateModel(expectedSurchargeRate);
            _surchargeRateServiceMock
                .Setup(s => s.FindByProductTypeAsync(It.IsAny<int>()))
                .ReturnsAsync(productInsurance);

            //Act
            var result = await _sut.GetAsync(It.IsAny<int>());

            //Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            var response = okResult.Value as SurchargeRateDto;
            Assert.Equal(expectedSurchargeRate, response.Value);
        }

        private SurchargeRateDto CreateSurchargeRateModel(float surchargeRate)
        {
            return new()
            {
                ProductTypeId = 123,
                Value = surchargeRate
            };
        }
    }
}
