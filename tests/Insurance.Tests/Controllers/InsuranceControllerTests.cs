using Insurance.Api.Controllers;
using Insurance.Application.Dto;
using Insurance.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Insurance.Tests.Controllers
{
    public class InsuranceControllerTests
    {
        private readonly Mock<IInsuranceService> _insuranceServiceMock;
        private readonly InsuranceController _sut;

        public InsuranceControllerTests()
        {
            _insuranceServiceMock = new Mock<IInsuranceService>();
            _sut = new InsuranceController(_insuranceServiceMock.Object);
        }

        [Fact]
        public async Task GetProductInsurance_Should_Return_NotFound_When_ProductIdInValid()
        {
            //Arrange
            _insuranceServiceMock.Setup(s => s.GetInsuranceByProductAsync(It.IsAny<int>()));

            //Act
            var result = await _sut.GetProductInsurance(It.IsAny<int>());

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetProductInsurance_Should_Return_ProductInsurance_WhenValidProductIdProvided()
        { 
            //Arrange
            float expectedInsuranceAmount = 123;
            var productInsurance = CreateProductInsuranceResponse(expectedInsuranceAmount);
            _insuranceServiceMock
                .Setup(s => s.GetInsuranceByProductAsync(It.IsAny<int>()))
                .ReturnsAsync(productInsurance);

            //Act
            var result = await _sut.GetProductInsurance(It.IsAny<int>());

            //Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            var productInsuranceResponse = okResult.Value as ProductInsuranceResponse;
            Assert.Equal(expectedInsuranceAmount, productInsuranceResponse.InsuranceValue);
        }

        [Fact]
        public async Task GetInsurance_Should_Return_NotFound_When_ProductIdNotPassed()
        {
            //Arrange
            _insuranceServiceMock.Setup(s => s.GetInsuranceByOrderAsync(It.IsAny<int[]>()));

            //Act
            var result = await _sut.GetOrderInsurance(It.IsAny<int[]>());

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetInsurance_Should_Return_NotFound_When_ProductIdInValid()
        {
            //Arrange
            _insuranceServiceMock.Setup(s => s.GetInsuranceByOrderAsync(It.IsAny<int[]>()));

            //Act
            var result = await _sut.GetOrderInsurance(new int[] { 1 });

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetOrderInsurance_Should_Return_Insurance_WhenValidOrderProvided()
        {
            //Arrange
            float expectedInsuranceAmount = 1234;
            var orderInsurance = CreateOrderInsuranceResponse(expectedInsuranceAmount);

            _insuranceServiceMock
                .Setup(s => s.GetInsuranceByOrderAsync(It.IsAny<int[]>()))
                .ReturnsAsync(orderInsurance);

            //Act
            var result = await _sut.GetOrderInsurance(new int[] { 1 });

            //Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            var productInsuranceResponse = okResult.Value as OrderInsuranceResponse;
            Assert.Equal(expectedInsuranceAmount, productInsuranceResponse.TotalInsurance);
        }

        private ProductInsuranceResponse CreateProductInsuranceResponse(float insuranceAmount)
        {
            return new()
            {
                InsuranceValue = insuranceAmount,
                ProductId = 123,
                ProductTypeId = 45
            };
        }

        private OrderInsuranceResponse CreateOrderInsuranceResponse(float insuranceAmount)
        {
            return new() { TotalInsurance = insuranceAmount };
        }
    }
}
