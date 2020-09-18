using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Darooha.Common.ErrorsAndMessages;
using Darooha.Common.Helpers.Interface;
using Darooha.Data.Dtos.Common.Token;
using Darooha.Data.Dtos.Site.Panel.Auth;
using Darooha.Data.Dtos.Site.Panel.User;
using Darooha.Data.Models;
using Darooha.Presentation.Controllers.Site.V1.Auth;
using Darooha.Services.Site.Admin.Auth.Interface;
using Darooha.Test.DataInput;
using Darooha.Test.IntegrationTests.Providers;
using Darooha.Test.UnitTests.Providers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Darooha.Test.UnitTests.ControllersTests
{
    public class AuthControllerUnitTests
    {
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IAuthService> _mockAuthService;
        private readonly Mock<ILogger<AuthController>> _mockLogger;
        private readonly Mock<IUtilities> _mockUtilities;
        private readonly Mock<FakeUserManager> _mockUserManager;
        private readonly AuthController _controller;

        public AuthControllerUnitTests()
        {
            _mockMapper = new Mock<IMapper>();
            _mockAuthService = new Mock<IAuthService>();
            _mockLogger = new Mock<ILogger<AuthController>>();
            _mockUserManager = new Mock<FakeUserManager>();
            _mockUtilities = new Mock<IUtilities>();

            _controller = new AuthController(_mockAuthService.Object, _mockMapper.Object,
                _mockLogger.Object, _mockUtilities.Object, _mockUserManager.Object);
        }

        #region loginTests
        [Fact]
        public async Task Login_Success_Passsword()
        {
            //Arrange------------------------------------------------------------------------------------------------------------------------------

            _mockUtilities.Setup(x => x.GenerateNewTokenAsync(It.IsAny<TokenRequestDTO>()))
                .ReturnsAsync(new TokenResponseDTO() { status = true });

            //Act----------------------------------------------------------------------------------------------------------------------------------
            var result = await _controller.Login(UnitTestsDataInput.useForLoginDto_Success_password);
            var okResult = result as OkObjectResult;
            //Assert-------------------------------------------------------------------------------------------------------------------------------
            Assert.NotNull(okResult);
            Assert.IsType<LoginResponseDTO>(okResult.Value);
            Assert.Equal(200, okResult.StatusCode);
        }
        [Fact]
        public async Task Login_Success_RefreshToken()
        {
            //Arrange------------------------------------------------------------------------------------------------------------------------------

            _mockUtilities.Setup(x => x.RefreshAccessTokenAsync(It.IsAny<TokenRequestDTO>()))
                .ReturnsAsync(new TokenResponseDTO() { status = true });

            //Act----------------------------------------------------------------------------------------------------------------------------------
            var result = await _controller.Login(UnitTestsDataInput.useForLoginDto_Success_refreshToken);
            var okResult = result as OkObjectResult;
            //Assert-------------------------------------------------------------------------------------------------------------------------------
            Assert.NotNull(okResult);
            Assert.IsType<TokenResponseDTO>(okResult.Value);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task Login_Fail_Passsword()
        {
            //Arrange------------------------------------------------------------------------------------------------------------------------------

            _mockUtilities.Setup(x => x.GenerateNewTokenAsync(It.IsAny<TokenRequestDTO>()))
                .ReturnsAsync(new TokenResponseDTO() { status = false, message = "کاربری با این یوزر و پس وجود ندارد" });
            string expected = "کاربری با این یوزر و پس وجود ندارد";



            //Act----------------------------------------------------------------------------------------------------------------------------------
            var result = await _controller.Login(UnitTestsDataInput.useForLoginDto_Fail_password);
            var okResult = result as UnauthorizedObjectResult;
            //Assert-------------------------------------------------------------------------------------------------------------------------------
            Assert.NotNull(okResult);
            Assert.IsType<string>(okResult.Value);
            Assert.Equal(expected, okResult.Value);
            Assert.Equal(401, okResult.StatusCode);
        }
        [Fact]
        public async Task Login_Fail_RefreshToken()
        {
            //Arrange------------------------------------------------------------------------------------------------------------------------------

            _mockUtilities.Setup(x => x.RefreshAccessTokenAsync(It.IsAny<TokenRequestDTO>()))
                .ReturnsAsync(new TokenResponseDTO() { status = false, message = "خطا در اعتبار سنجی خودکار" });
            string expected = "خطا در اعتبار سنجی خودکار";



            //Act----------------------------------------------------------------------------------------------------------------------------------
            var result = await _controller.Login(UnitTestsDataInput.useForLoginDto_Fail_refreshToken);
            var okResult = result as UnauthorizedObjectResult;
            //Assert-------------------------------------------------------------------------------------------------------------------------------
            Assert.NotNull(okResult);
            Assert.IsType<string>(okResult.Value);
            Assert.Equal(expected, okResult.Value);
            Assert.Equal(401, okResult.StatusCode);
        }
        [Fact]
        public async Task Login_Fail_ModelStateError()
        {
            //Arrange------------------------------------------------------------------------------------------------------------------------------
            var controller = new ModelStateControllerTests();
            //Act----------------------------------------------------------------------------------------------------------------------------------
            controller.ValidateModelState(UnitTestsDataInput.useForLoginDto_Fail_ModelState);
            var modelState = controller.ModelState;
            //Assert-------------------------------------------------------------------------------------------------------------------------------
            Assert.False(modelState.IsValid);
            Assert.Equal(2, modelState.Keys.Count());
            Assert.True(modelState.Keys.Contains("UserName") && modelState.Keys.Contains("GrantType"));
        }
        #endregion

        #region registerTests
        [Fact]
        public async Task Register_Success()
        {
            //Arrange------------------------------------------------------------------------------------------------------------------------------
            _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<Tbl_User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            _mockAuthService.Setup(x => x.AddUserPreNeededAsync(It.IsAny<Notification>()));

            _mockMapper.Setup(x => x.Map<UserForDetailedDTO>(It.IsAny<Tbl_User>()))
                .Returns(UnitTestsDataInput.GetUserForDetailedDto);
            //Act----------------------------------------------------------------------------------------------------------------------------------

            //Assert-------------------------------------------------------------------------------------------------------------------------------
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Scheme = "222";
            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };

            var result = await _controller.Register(UnitTestsDataInput.userForRegisterDto);
            var okResult = result as CreatedAtRouteResult;
            //Assert-------------------------------------------------------------------------------------------------------------------------------
            Assert.NotNull(okResult);
            Assert.IsType<UserForDetailedDTO>(okResult.Value);
            Assert.Equal(201, okResult.StatusCode);
        }
        [Fact]
        public async Task Register_Fail_UserExist()
        {
            //Arrange------------------------------------------------------------------------------------------------------------------------------
            _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<Tbl_User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed());

            //Act----------------------------------------------------------------------------------------------------------------------------------

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Scheme = "222";
            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };
            var result = await _controller.Register(UnitTestsDataInput.userForRegisterDto);
            var okResult = result as BadRequestObjectResult;
            //Assert-------------------------------------------------------------------------------------------------------------------------------

            Assert.NotNull(okResult);
            Assert.IsType<ReturnErrorMessage>(okResult.Value);
            Assert.Equal(400, okResult.StatusCode);
        }
        [Fact]
        public void Register_Fail_ModelStateError()
        {
            //Arrange------------------------------------------------------------------------------------------------------------------------------

            var controller = new ModelStateControllerTests();
            //Act----------------------------------------------------------------------------------------------------------------------------------

            controller.ValidateModelState(UnitTestsDataInput.userForRegisterDto_Fail_ModelState);
            var modelState = controller.ModelState;
            //Assert-------------------------------------------------------------------------------------------------------------------------------

            Assert.False(modelState.IsValid);
            Assert.Equal(5, modelState.Keys.Count());
            Assert.True(modelState.Keys.Contains("Email") && modelState.Keys.Contains("Password")
                        && modelState.Keys.Contains("FirstName") && modelState.Keys.Contains("LastName")
                         && modelState.Keys.Contains("MobPhone"));
        }
        #endregion
    }
}
