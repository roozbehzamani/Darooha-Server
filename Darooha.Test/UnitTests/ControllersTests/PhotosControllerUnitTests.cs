using AutoMapper;
using Darooha.Common.ErrorsAndMessages;
using Darooha.Data.DatabaseContext;
using Darooha.Data.Dtos.Site.Panel.Photo;
using Darooha.Data.Models;
using Darooha.Presentation.Controllers.Site.V1.User;
using Darooha.Repo.Infrastructure;
using Darooha.Services.Upload.Interface;
using Darooha.Test.DataInput;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Routing;

namespace Darooha.Test.UnitTests.ControllersTests
{
    public class PhotosControllerUnitTests
    {
        private readonly Mock<IUnitOfWork<DaroohaDbContext>> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IUploadService> _mockUploadService;
        private readonly Mock<IWebHostEnvironment> _mockWebHostEnvironment;
        private readonly PhotosController _controller;
        private readonly Mock<ILogger<PhotosController>> _mockLogger;

        public PhotosControllerUnitTests()
        {
            _mockRepo = new Mock<IUnitOfWork<DaroohaDbContext>>();
            _mockMapper = new Mock<IMapper>();
            _mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            _mockUploadService = new Mock<IUploadService>();
            _mockLogger = new Mock<ILogger<PhotosController>>();
            _controller = new PhotosController(_mockRepo.Object, _mockMapper.Object, _mockUploadService.Object,
                _mockWebHostEnvironment.Object, _mockLogger.Object);

        }

        #region GetPhotoTests
        [Fact]
        public async Task GetPhoto_Success()
        {
            //Arrange------------------------------------------------------------------------------------------------------------------------------
            _mockRepo.Setup(x => x.UserRepository.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(UnitTestsDataInput.GetUser.First());

            _mockMapper.Setup(x => x.Map<PhotoForReturnProfileDTO>(It.IsAny<Tbl_User>()))
                .Returns(UnitTestsDataInput.PhotoForReturnProfileDto);

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
            var result = await _controller.GetPhoto(It.IsAny<string>());
            var okResult = result as OkObjectResult;
            //Assert-------------------------------------------------------------------------------------------------------------------------------
            Assert.NotNull(okResult);
            Assert.IsType<PhotoForReturnProfileDTO>(okResult.Value);
            Assert.Equal(200, okResult.StatusCode);
        }
        [Fact]
        public async Task GetPhoto_Fail_SeeAnOtherOnePhoto()
        {
            //Arrange------------------------------------------------------------------------------------------------------------------------------
            _mockRepo.Setup(x => x.UserRepository.GetByIdAsync(It.IsAny<string>()))
               .ReturnsAsync(UnitTestsDataInput.GetUser.First());


            var rout = new RouteData();
            rout.Values.Add("userId", UnitTestsDataInput.GetUser.First().Id);

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
            var result = await _controller.GetPhoto(It.IsAny<string>());
            var okResult = result as BadRequestObjectResult;
            //Assert-------------------------------------------------------------------------------------------------------------------------------
            Assert.NotNull(okResult);
            Assert.IsType<ReturnErrorMessage>(okResult.Value);
            Assert.Equal(400, okResult.StatusCode);
        }
        #endregion

        #region ChangeUserPhotoTests
        [Fact]
        public async Task ChangeUserPhoto_Success()
        {
            //Arrange------------------------------------------------------------------------------------------------------------------------------
            _mockRepo.Setup(x => x.UserRepository.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(UnitTestsDataInput.GetUser.First());

            _mockUploadService.Setup(x => x.UploadProfilePic(It.IsAny<IFormFile>(),
                    It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(UnitTestsDataInput.fileUploadedDto_Success);

            _mockRepo.Setup(x => x.UserRepository.Update(It.IsAny<Tbl_User>()));

            _mockRepo.Setup(x => x.SaveAsync()).ReturnsAsync(true);

            _mockWebHostEnvironment.Setup(x => x.WebRootPath).Returns(It.IsAny<string>());

            _mockMapper.Setup(x => x.Map<PhotoForReturnProfileDTO>(It.IsAny<Tbl_User>()))
                .Returns(UnitTestsDataInput.PhotoForReturnProfileDto);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Scheme = "222";
            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };


            //Act----------------------------------------------------------------------------------------------------------------------------------
            var result = await _controller.ChangeUserPhoto(It.IsAny<string>(), UnitTestsDataInput.photoForProfileDto);
            var okResult = result as CreatedAtRouteResult;
            //Assert-------------------------------------------------------------------------------------------------------------------------------
            Assert.NotNull(okResult);
            Assert.IsType<PhotoForReturnProfileDTO>(okResult.Value);
            Assert.Equal(201, okResult.StatusCode);
        }
        [Fact]
        public async Task ChangeUserPhoto_Fail_WorngFile()
        {
            //Arrange------------------------------------------------------------------------------------------------------------------------------
            _mockRepo.Setup(x => x.UserRepository.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(UnitTestsDataInput.GetUser.First());

            _mockUploadService.Setup(x => x.UploadProfilePic(It.IsAny<IFormFile>(),
                    It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(UnitTestsDataInput.fileUploadedDto_Fail_WrongFile);

            _mockRepo.Setup(x => x.UserRepository.Update(It.IsAny<Tbl_User>()));

            _mockRepo.Setup(x => x.SaveAsync()).ReturnsAsync(true);

            _mockWebHostEnvironment.Setup(x => x.WebRootPath).Returns(It.IsAny<string>());

            _mockMapper.Setup(x => x.Map<PhotoForReturnProfileDTO>(It.IsAny<Tbl_User>()))
                .Returns(UnitTestsDataInput.PhotoForReturnProfileDto);

            _mockWebHostEnvironment.Setup(x => x.WebRootPath).Returns(It.IsAny<string>());


            var httpContext = new DefaultHttpContext();
            httpContext.Request.Scheme = "222";
            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };

            //Act----------------------------------------------------------------------------------------------------------------------------------
            var result = await _controller.ChangeUserPhoto(It.IsAny<string>(), UnitTestsDataInput.photoForProfileDto);
            var okResult = result as BadRequestObjectResult;
            //Assert-------------------------------------------------------------------------------------------------------------------------------
            Assert.NotNull(okResult);
            Assert.IsType<ReturnErrorMessage>(okResult.Value);
            Assert.Equal(400, okResult.StatusCode);
        }

        #endregion
    }
}
