using AutoMapper;
using Darooha.Data.DatabaseContext;
using Darooha.Data.Dtos.Site.Panel.UserAddress;
using Darooha.Data.Models;
using Darooha.Presentation.Controllers.Site.V1.User;
using Darooha.Repo.Infrastructure;
using Darooha.Test.DataInput;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace Darooha.Test.UnitTests.ControllersTests
{
    public class UserAddressControllerUnitTest
    {
        private readonly Mock<IUnitOfWork<DaroohaDbContext>> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<UserAddressController>> _mockLogger;
        private readonly UserAddressController _controller;

        public UserAddressControllerUnitTest()
        {
            _mockRepo = new Mock<IUnitOfWork<DaroohaDbContext>>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<UserAddressController>>();
            _controller = new UserAddressController(_mockRepo.Object, _mockMapper.Object, _mockLogger.Object);
        }

        #region GetUserAddressTests
        [Fact]
        public async Task GetUserAddress_Success()
        {
            //Arrange------------------------------------------------------------------------------------------------------------------------------
            _mockRepo.Setup(x => x.UserAddressRepository.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(UnitTestsDataInput.GetUser.First().Tbl_UserAddresses.First());

            _mockMapper.Setup(x => x.Map<UserAddressForReturnDTO>(It.IsAny<Tbl_UserAddress>()))
                .Returns(UnitTestsDataInput.userAddressForReturnDTO_Success);

            var rout = new RouteData();
            rout.Values.Add("userId", UnitTestsDataInput.GetUser.First().Id);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,UnitTestsDataInput.currentUserId),
            };
            var identity = new ClaimsIdentity(claims);
            var claimsPrincipal = new ClaimsPrincipal(identity);
            var mockContext = new Mock<HttpContext>();

            mockContext.SetupGet(x => x.User).Returns(claimsPrincipal);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = mockContext.Object,
                RouteData = rout
            };

            //Act----------------------------------------------------------------------------------------------------------------------------------
            var result = await _controller.GetUserAddress(It.IsAny<string>(), It.IsAny<string>());
            var okResult = result as OkObjectResult;
            //Assert-------------------------------------------------------------------------------------------------------------------------------
            Assert.NotNull(okResult);
            Assert.IsType<UserAddressForReturnDTO>(okResult.Value);
            Assert.Equal(200, okResult.StatusCode);
        }
        [Fact]
        public async Task GetUserAddress_Fail()
        {
            //Arrange------------------------------------------------------------------------------------------------------------------------------
            _mockRepo.Setup(x => x.UserAddressRepository.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(UnitTestsDataInput.GetUser.First().Tbl_UserAddresses.First());

            var rout = new RouteData();
            rout.Values.Add("userId", UnitTestsDataInput.currentUserId);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,UnitTestsDataInput.userAnOtherId),
            };
            var identity = new ClaimsIdentity(claims);
            var claimsPrincipal = new ClaimsPrincipal(identity);
            var mockContext = new Mock<HttpContext>();

            mockContext.SetupGet(x => x.User).Returns(claimsPrincipal);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = mockContext.Object,
                RouteData = rout
            };



            //Act----------------------------------------------------------------------------------------------------------------------------------
            var result = await _controller.GetUserAddress(It.IsAny<string>(), It.IsAny<string>());
            var okResult = result as BadRequestObjectResult;
            //Assert-------------------------------------------------------------------------------------------------------------------------------
            Assert.NotNull(okResult);
            Assert.IsType<string>(okResult.Value);
            Assert.Equal(400, okResult.StatusCode);
        }
        #endregion

        #region UpdateUserAddressTests
        [Fact]
        public async Task UpdateUserAddress_Success()
        {
            //Arrange------------------------------------------------------------------------------------------------------------------------------
            _mockRepo.Setup(x => x.UserAddressRepository.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(UnitTestsDataInput.GetUser.First().Tbl_UserAddresses.First());

            _mockRepo.Setup(x => x.UserAddressRepository.Update(It.IsAny<Tbl_UserAddress>()));

            _mockRepo.Setup(x => x.SaveAsync()).ReturnsAsync(true);

            _mockMapper.Setup(x => x.Map(It.IsAny<UserAddressForUpdateDTO>(), It.IsAny<Tbl_UserAddress>()))
                .Returns(new Tbl_UserAddress());

            var rout = new RouteData();
            rout.Values.Add("userId", UnitTestsDataInput.GetUser.First().Id);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,UnitTestsDataInput.currentUserId),
            };
            var identity = new ClaimsIdentity(claims);
            var claimsPrincipal = new ClaimsPrincipal(identity);
            var mockContext = new Mock<HttpContext>();

            mockContext.SetupGet(x => x.User).Returns(claimsPrincipal);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = mockContext.Object,
                RouteData = rout
            };

            //Act----------------------------------------------------------------------------------------------------------------------------------
            var result = await _controller.UpdateUserAddress(It.IsAny<string>(), It.IsAny<UserAddressForUpdateDTO>());
            var okResult = result as NoContentResult;
            //Assert-------------------------------------------------------------------------------------------------------------------------------
            Assert.Equal(204, okResult.StatusCode);
        }
        [Fact]
        public async Task UpdateUserAddress_Fail()
        {
            //Arrange------------------------------------------------------------------------------------------------------------------------------
            _mockRepo.Setup(x => x.UserAddressRepository.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(UnitTestsDataInput.GetUser.First().Tbl_UserAddresses.First());


            var rout = new RouteData();
            rout.Values.Add("userId", UnitTestsDataInput.GetUser.First().Id);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,UnitTestsDataInput.currentUserId),
            };
            var identity = new ClaimsIdentity(claims);
            var claimsPrincipal = new ClaimsPrincipal(identity);
            var mockContext = new Mock<HttpContext>();

            mockContext.SetupGet(x => x.User).Returns(claimsPrincipal);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = mockContext.Object,
                RouteData = rout
            };

            //Act----------------------------------------------------------------------------------------------------------------------------------
            var result = await _controller.UpdateUserAddress(It.IsAny<string>(), It.IsAny<UserAddressForUpdateDTO>());
            var okResult = result as BadRequestObjectResult;
            //Assert-------------------------------------------------------------------------------------------------------------------------------
            Assert.NotNull(okResult);
            Assert.IsType<string>(okResult.Value);
            Assert.Equal(400, okResult.StatusCode);
        }
        #endregion

        #region DeleteUserAddressTests
        [Fact]
        public async Task DeleteUserAddress_Success()
        {
            //Arrange------------------------------------------------------------------------------------------------------------------------------
            _mockRepo.Setup(x => x.UserAddressRepository.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(UnitTestsDataInput.GetUser.First().Tbl_UserAddresses.First());

            _mockRepo.Setup(x => x.UserAddressRepository.Delete(It.IsAny<Tbl_UserAddress>()));

            _mockRepo.Setup(x => x.SaveAsync()).ReturnsAsync(true);

            var rout = new RouteData();
            rout.Values.Add("userId", UnitTestsDataInput.GetUser.First().Id);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,UnitTestsDataInput.currentUserId),
            };
            var identity = new ClaimsIdentity(claims);
            var claimsPrincipal = new ClaimsPrincipal(identity);
            var mockContext = new Mock<HttpContext>();

            mockContext.SetupGet(x => x.User).Returns(claimsPrincipal);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = mockContext.Object,
                RouteData = rout
            };

            //Act----------------------------------------------------------------------------------------------------------------------------------
            var result = await _controller.DeleteUserAddress(It.IsAny<string>());
            var okResult = result as NoContentResult;
            //Assert-------------------------------------------------------------------------------------------------------------------------------
            Assert.Equal(204, okResult.StatusCode);
        }
        [Fact]
        public async Task DeleteUserAddress_Fail()
        {
            //Arrange------------------------------------------------------------------------------------------------------------------------------
            _mockRepo.Setup(x => x.UserAddressRepository.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(UnitTestsDataInput.GetUser.First().Tbl_UserAddresses.First());


            var rout = new RouteData();
            rout.Values.Add("userId", UnitTestsDataInput.GetUser.First().Id);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,UnitTestsDataInput.currentUserId),
            };
            var identity = new ClaimsIdentity(claims);
            var claimsPrincipal = new ClaimsPrincipal(identity);
            var mockContext = new Mock<HttpContext>();

            mockContext.SetupGet(x => x.User).Returns(claimsPrincipal);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = mockContext.Object,
                RouteData = rout
            };

            //Act----------------------------------------------------------------------------------------------------------------------------------
            var result = await _controller.DeleteUserAddress(It.IsAny<string>());
            var okResult = result as BadRequestObjectResult;
            //Assert-------------------------------------------------------------------------------------------------------------------------------
            Assert.NotNull(okResult);
            Assert.IsType<string>(okResult.Value);
            Assert.Equal(400, okResult.StatusCode);
        }
        #endregion

        #region AddUserAddressTests
        [Fact]
        public async Task AddUserAddress_Success()
        {
            //Arrange------------------------------------------------------------------------------------------------------------------------------
            _mockRepo.Setup(x => x.UserAddressRepository.GetAsync(It.IsAny<Expression<Func<Tbl_UserAddress, bool>>>()))
                .ReturnsAsync(It.IsAny<Tbl_UserAddress>());

            _mockRepo.Setup(x => x.UserAddressRepository.InsertAsync(It.IsAny<Tbl_UserAddress>()));

            _mockRepo.Setup(x => x.SaveAsync()).ReturnsAsync(true);

            _mockMapper.Setup(x => x.Map(It.IsAny<UserAddressForCreateDTO>(), It.IsAny<Tbl_UserAddress>()))
                .Returns(new Tbl_UserAddress());

            //Act----------------------------------------------------------------------------------------------------------------------------------
            var result = await _controller.AddUserAddress(It.IsAny<string>(), UnitTestsDataInput.userAddressForCreateDTO_Success);
            var okResult = result as CreatedAtRouteResult;
            //Assert-------------------------------------------------------------------------------------------------------------------------------
            Assert.NotNull(okResult);
            Assert.IsType<Tbl_UserAddress>(okResult.Value);
            Assert.Equal(201, okResult.StatusCode);
        }
        [Fact]
        public async Task AddUserAddress_Fail_DuplicateAddress()
        {
            //Arrange------------------------------------------------------------------------------------------------------------------------------
            _mockRepo.Setup(x => x.UserAddressRepository.GetAsync(It.IsAny<Expression<Func<Tbl_UserAddress, bool>>>()))
                .ReturnsAsync(new Tbl_UserAddress());

            //Act----------------------------------------------------------------------------------------------------------------------------------
            var result = await _controller.AddUserAddress(It.IsAny<string>(), UnitTestsDataInput.userAddressForCreateDTO_Success);
            var okResult = result as BadRequestObjectResult;
            //Assert-------------------------------------------------------------------------------------------------------------------------------
            Assert.NotNull(okResult);
            Assert.IsType<string>(okResult.Value);
            Assert.Equal(400, okResult.StatusCode);
        }

        #endregion
    }
}
