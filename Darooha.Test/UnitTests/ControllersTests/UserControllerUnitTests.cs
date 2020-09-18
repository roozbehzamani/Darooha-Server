using AutoMapper;
using Darooha.Common.ErrorsAndMessages;
using Darooha.Data.DatabaseContext;
using Darooha.Data.Dtos.Site.Panel.User;
using Darooha.Data.Models;
using Darooha.Presentation.Controllers.Site.V1.User;
using Darooha.Repo.Infrastructure;
using Darooha.Services.Site.Admin.User.Interface;
using Darooha.Test.DataInput;
using Darooha.Test.IntegrationTests.Providers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace Darooha.Test.UnitTests.ControllersTests
{
    public class UserControllerUnitTests
    {
        private readonly Mock<IUnitOfWork<DaroohaDbContext>> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IUserService> _mockUserService;
        private readonly Mock<ILogger<UserController>> _mockLogger;
        private readonly UserController _controller;

        public UserControllerUnitTests()
        {
            _mockRepo = new Mock<IUnitOfWork<DaroohaDbContext>>();
            _mockMapper = new Mock<IMapper>();
            _mockUserService = new Mock<IUserService>();
            _mockLogger = new Mock<ILogger<UserController>>();
            _controller = new UserController(_mockRepo.Object, _mockMapper.Object, _mockUserService.Object, _mockLogger.Object);
        }

        #region GetUserTests
        [Fact]
        public async Task GetUser_Success()
        {
            //Arrange------------------------------------------------------------------------------------------------------------------------------
            var users = UnitTestsDataInput.GetUser;
            var userForDetailedDto = UnitTestsDataInput.GetUserForDetailedDto();
            _mockRepo.Setup(x => x.UserRepository
                .GetManyAsync(
                    It.IsAny<Expression<Func<Tbl_User, bool>>>(),
                    It.IsAny<Func<IQueryable<Tbl_User>, IOrderedQueryable<Tbl_User>>>(),
                    It.IsAny<string>())).ReturnsAsync(() => users);
            //
            _mockMapper.Setup(x => x.Map<UserForDetailedDTO>(It.IsAny<Tbl_User>()))
                .Returns(userForDetailedDto);

            //Act----------------------------------------------------------------------------------------------------------------------------------
            var result = await _controller.GetUser(It.IsAny<string>());
            var okResult = result as OkObjectResult;
            //Assert-------------------------------------------------------------------------------------------------------------------------------

            Assert.NotNull(okResult);
            Assert.IsType<UserForDetailedDTO>(okResult.Value);
            Assert.Equal(200, okResult.StatusCode);
        }

        #endregion

        #region UpdateUserTests
        [Fact]
        public async Task UpdateUser_Success()
        {
            //Arrange------------------------------------------------------------------------------------------------------------------------------
            var users = UnitTestsDataInput.GetUser;
            //var userForDetailedDto = UsersControllerMockData.GetUserForDetailedDto();
            _mockRepo.Setup(x => x.UserRepository
                .GetByIdAsync(
                    It.IsAny<string>())).ReturnsAsync(() => users.First());

            _mockRepo.Setup(x => x.UserRepository
                .Update(
                    It.IsAny<Tbl_User>()));

            _mockRepo.Setup(x => x.SaveAsync()).ReturnsAsync(true);
            //
            _mockMapper.Setup(x => x.Map(It.IsAny<UserForUpdateDTO>(), It.IsAny<Tbl_User>()))
                .Returns(users.First());

            //Act----------------------------------------------------------------------------------------------------------------------------------

            var result = await _controller.UpdateUser(It.IsAny<string>(), It.IsAny<UserForUpdateDTO>());
            var okResult = result as NoContentResult;
            //Assert-------------------------------------------------------------------------------------------------------------------------------

            Assert.NotNull(okResult);
            Assert.Equal(204, okResult.StatusCode);
        }
        [Fact]
        public async Task UpdateUser_Fail()
        {
            //Arrange------------------------------------------------------------------------------------------------------------------------------
            var users = UnitTestsDataInput.GetUser;
            //var userForDetailedDto = UsersControllerMockData.GetUserForDetailedDto();
            _mockRepo.Setup(x => x.UserRepository
                .GetByIdAsync(
                    It.IsAny<string>())).ReturnsAsync(() => users.First());

            _mockRepo.Setup(x => x.UserRepository
                .Update(
                    It.IsAny<Tbl_User>()));

            _mockRepo.Setup(x => x.SaveAsync()).ReturnsAsync(false);
            //
            _mockMapper.Setup(x => x.Map(It.IsAny<UserForUpdateDTO>(), It.IsAny<Tbl_User>()))
                .Returns(users.First());

            //Act----------------------------------------------------------------------------------------------------------------------------------

            var result = await _controller.UpdateUser(It.IsAny<string>(), It.IsAny<UserForUpdateDTO>());
            var badResult = result as BadRequestObjectResult;
            //Assert-------------------------------------------------------------------------------------------------------------------------------

            Assert.NotNull(badResult);
            Assert.IsType<ReturnErrorMessage>(badResult.Value);
            Assert.Equal(400, badResult.StatusCode);
        }
        #endregion

        #region ChangeUserPasswordTests
        [Fact]
        public async Task ChangeUserPassword_Success()
        {
            //Arrange------------------------------------------------------------------------------------------------------------------------------

            _mockUserService.Setup(x => x.GetUserForPassChange(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(UnitTestsDataInput.GetUser.First());

            //Act----------------------------------------------------------------------------------------------------------------------------------
            _mockUserService.Setup(x => x.UpdateUserPassword(It.IsAny<Tbl_User>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            //Assert-------------------------------------------------------------------------------------------------------------------------------
            //_mockUtilities.Setup(x => x.VerifyPasswordHash(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<byte[]>()))
            //    .Returns(true);

            //byte[] passwordHash, passwordSalt;
            //_mockUtilities.Setup(x => x.CreatePasswordHash(It.IsAny<string>(),out passwordHash, out passwordSalt));

            var result = await _controller.ChangeUserPassword(It.IsAny<string>(), UnitTestsDataInput.passwordForChangeDto);
            var okResult = result as NoContentResult;
            //Assert-------------------------------------------------------------------------------------------------------------------------------
            Assert.NotNull(okResult);
            Assert.Equal(204, okResult.StatusCode);
        }
        [Fact]
        public async Task ChangeUserPassword_Fail_WrongOldPassword()
        {
            //Arrange------------------------------------------------------------------------------------------------------------------------------

            _mockUserService.Setup(x => x.GetUserForPassChange(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(It.IsAny<Tbl_User>());

            //Act----------------------------------------------------------------------------------------------------------------------------------

            var result = await _controller.ChangeUserPassword(It.IsAny<string>(), UnitTestsDataInput.passwordForChangeDto);
            var badResult = result as BadRequestObjectResult;
            //Assert-------------------------------------------------------------------------------------------------------------------------------

            Assert.NotNull(badResult);
            Assert.IsType<ReturnErrorMessage>(badResult.Value);
            Assert.Equal(400, badResult.StatusCode);
        }
        [Fact]
        public void ChangeUserPassword_Fail_ModelStateError()
        {
            //Arrange------------------------------------------------------------------------------------------------------------------------------

            var controller = new ModelStateControllerTests();

            //Act----------------------------------------------------------------------------------------------------------------------------------

            controller.ValidateModelState(UnitTestsDataInput.passwordForChangeDto_Fail_ModelState);
            var modelState = controller.ModelState;
            //Assert-------------------------------------------------------------------------------------------------------------------------------

            Assert.False(modelState.IsValid);
            Assert.Equal(2, modelState.Keys.Count());
            Assert.True(modelState.Keys.Contains("OldPassword") && modelState.Keys.Contains("NewPassword"));
        }
        #endregion
    }
}
