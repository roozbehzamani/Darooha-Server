using AutoMapper;
using Darooha.Data.DatabaseContext;
using Darooha.Data.Dtos.Site.Panel.Notification;
using Darooha.Data.Models;
using Darooha.Presentation.Controllers.Site.V1.User;
using Darooha.Repo.Infrastructure;
using Darooha.Test.DataInput;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace Darooha.Test.UnitTests.ControllersTests
{
    public class NotificationsControllerUnitTests
    {
        private readonly Mock<IUnitOfWork<DaroohaDbContext>> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<NotificationsController>> _mockLogger;
        private readonly NotificationsController _controller;

        public NotificationsControllerUnitTests()
        {
            _mockRepo = new Mock<IUnitOfWork<DaroohaDbContext>>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<NotificationsController>>();
            _controller = new NotificationsController(_mockRepo.Object, _mockLogger.Object, _mockMapper.Object);
        }

        #region GetUserTests
        [Fact]
        public async Task UpdateUserNotify_Success()
        {
            //Arrange------------------------------------------------------------------------------------------------------------------------------
            _mockRepo.Setup(x => x.NotificationRepository.GetManyAsync(
                It.IsAny<Expression<Func<Notification, bool>>>(),
                It.IsAny<Func<IQueryable<Notification>, IOrderedQueryable<Notification>>>(),
                It.IsAny<string>()))
                .ReturnsAsync(UnitTestsDataInput.notify_Success);

            _mockRepo.Setup(x => x.NotificationRepository.Update(It.IsAny<Notification>()));

            _mockRepo.Setup(x => x.SaveAsync()).ReturnsAsync(true);


            //
            _mockMapper.Setup(x => x.Map(It.IsAny<NotificationForUpdateDTO>(), It.IsAny<Notification>()))
                .Returns(UnitTestsDataInput.notify_Success.First());

            //Act----------------------------------------------------------------------------------------------------------------------------------
            var result = await _controller.UpdateUserNotify(UnitTestsDataInput.currentUserId, UnitTestsDataInput.notifyForUpdate_Success);
            var okResult = result as NoContentResult;
            //Assert-------------------------------------------------------------------------------------------------------------------------------
            Assert.Equal(204, okResult.StatusCode);
        }

        [Fact]
        public async Task UpdateUserNotify_Success_CreateNotify()
        {
            //Arrange------------------------------------------------------------------------------------------------------------------------------
            _mockRepo.Setup(x => x.NotificationRepository.GetManyAsync(
                    It.IsAny<Expression<Func<Notification, bool>>>(),
                    It.IsAny<Func<IQueryable<Notification>, IOrderedQueryable<Notification>>>(),
                    It.IsAny<string>()))
                .ReturnsAsync(new List<Notification>());

            _mockRepo.Setup(x => x.NotificationRepository.InsertAsync(It.IsAny<Notification>()));

            _mockRepo.Setup(x => x.SaveAsync()).ReturnsAsync(true);


            //
            _mockMapper.Setup(x => x.Map(It.IsAny<NotificationForUpdateDTO>(), It.IsAny<Notification>()))
                .Returns(UnitTestsDataInput.notify_Success.First());

            //Act----------------------------------------------------------------------------------------------------------------------------------
            var result = await _controller.UpdateUserNotify(UnitTestsDataInput.currentUserId, UnitTestsDataInput.notifyForUpdate_Success);
            var okResult = result as NoContentResult;
            //Assert-------------------------------------------------------------------------------------------------------------------------------
            Assert.Equal(204, okResult.StatusCode);
        }

        [Fact]
        public async Task UpdateUserNotify_Fail()
        {
            //Arrange------------------------------------------------------------------------------------------------------------------------------
            _mockRepo.Setup(x => x.NotificationRepository.GetManyAsync(
                It.IsAny<Expression<Func<Notification, bool>>>(),
                It.IsAny<Func<IQueryable<Notification>, IOrderedQueryable<Notification>>>(),
                It.IsAny<string>()))
                .ReturnsAsync(UnitTestsDataInput.notify_Success);

            _mockRepo.Setup(x => x.NotificationRepository.Update(It.IsAny<Notification>()));

            _mockRepo.Setup(x => x.SaveAsync()).ReturnsAsync(false);


            //
            _mockMapper.Setup(x => x.Map(It.IsAny<NotificationForUpdateDTO>(), It.IsAny<Notification>()))
                .Returns(UnitTestsDataInput.notify_Success.First());

            string expected = "خطای ثبت در دیتابیس";
            //Act----------------------------------------------------------------------------------------------------------------------------------
            var result = await _controller.UpdateUserNotify(UnitTestsDataInput.currentUserId, UnitTestsDataInput.notifyForUpdate_Success);
            var okResult = result as BadRequestObjectResult;
            //Assert-------------------------------------------------------------------------------------------------------------------------------
            Assert.NotNull(okResult);
            Assert.IsType<string>(okResult.Value);
            Assert.Equal(expected, okResult.Value);
            Assert.Equal(400, okResult.StatusCode);
        }

        [Fact]
        public async Task UpdateUserNotify_Fail_CreateNotify()
        {
            //Arrange------------------------------------------------------------------------------------------------------------------------------
            _mockRepo.Setup(x => x.NotificationRepository.GetManyAsync(
                    It.IsAny<Expression<Func<Notification, bool>>>(),
                    It.IsAny<Func<IQueryable<Notification>, IOrderedQueryable<Notification>>>(),
                    It.IsAny<string>()))
                .ReturnsAsync(new List<Notification>());

            _mockRepo.Setup(x => x.NotificationRepository.InsertAsync(It.IsAny<Notification>()));

            _mockRepo.Setup(x => x.SaveAsync()).ReturnsAsync(false);


            //
            _mockMapper.Setup(x => x.Map(It.IsAny<NotificationForUpdateDTO>(), It.IsAny<Notification>()))
                .Returns(UnitTestsDataInput.notify_Success.First());

            string expected = "خطای ثبت در دیتابیس";
            //Act----------------------------------------------------------------------------------------------------------------------------------
            var result = await _controller.UpdateUserNotify(UnitTestsDataInput.currentUserId, UnitTestsDataInput.notifyForUpdate_Success);
            var okResult = result as BadRequestObjectResult;
            //Assert-------------------------------------------------------------------------------------------------------------------------------
            Assert.NotNull(okResult);
            Assert.IsType<string>(okResult.Value);
            Assert.Equal(expected, okResult.Value);
            Assert.Equal(400, okResult.StatusCode);
        }

        #endregion

        #region UpdateUserNotifyTests
        [Fact]
        public async Task GetUserNotify_Success_Himself()
        {
            //Arrange------------------------------------------------------------------------------------------------------------------------------
            _mockRepo.Setup(x => x.NotificationRepository.GetManyAsync(
                It.IsAny<Expression<Func<Notification, bool>>>(),
                It.IsAny<Func<IQueryable<Notification>, IOrderedQueryable<Notification>>>(),
                It.IsAny<string>()))
                .ReturnsAsync(UnitTestsDataInput.notify_Success);


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
                HttpContext = mockContext.Object
            };


            //Act----------------------------------------------------------------------------------------------------------------------------------
            var result = await _controller.GetUserNotify(UnitTestsDataInput.currentUserId);
            var okResult = result as OkObjectResult;
            //Assert-------------------------------------------------------------------------------------------------------------------------------
            Assert.NotNull(okResult);
            Assert.IsType<Notification>(okResult.Value);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task UetUserNotify_Fail_AnOtherOne()
        {
            //Arrange------------------------------------------------------------------------------------------------------------------------------
            _mockRepo.Setup(x => x.NotificationRepository.GetManyAsync(
                    It.IsAny<Expression<Func<Notification, bool>>>(),
                    It.IsAny<Func<IQueryable<Notification>, IOrderedQueryable<Notification>>>(),
                    It.IsAny<string>()))
                .ReturnsAsync(UnitTestsDataInput.notify_Success);

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
                HttpContext = mockContext.Object
            };

            string expected = $"شما اجازه دسترسی به این اطلاعات را ندارید";

            //Act----------------------------------------------------------------------------------------------------------------------------------
            var result = await _controller.GetUserNotify(UnitTestsDataInput.currentUserId);
            var okResult = result as UnauthorizedObjectResult;
            //Assert-------------------------------------------------------------------------------------------------------------------------------
            Assert.NotNull(okResult);
            Assert.IsType<string>(okResult.Value);
            Assert.Equal(expected, okResult.Value);
            Assert.Equal(401, okResult.StatusCode);
        }

        [Fact]
        public async Task GetUserNotify_Fail_NoNotify()
        {
            //Arrange------------------------------------------------------------------------------------------------------------------------------
            _mockRepo.Setup(x => x.NotificationRepository.GetManyAsync(
                    It.IsAny<Expression<Func<Notification, bool>>>(),
                    It.IsAny<Func<IQueryable<Notification>, IOrderedQueryable<Notification>>>(),
                    It.IsAny<string>()))
                .ReturnsAsync(new List<Notification>());

            string expected = "اطلاعات اطلاع رسانی وجود ندارد";

            //Act----------------------------------------------------------------------------------------------------------------------------------
            var result = await _controller.GetUserNotify(UnitTestsDataInput.currentUserId);
            var okResult = result as BadRequestObjectResult;
            //Assert-------------------------------------------------------------------------------------------------------------------------------
            Assert.NotNull(okResult);
            Assert.IsType<string>(okResult.Value);
            Assert.Equal(expected, okResult.Value);
            Assert.Equal(400, okResult.StatusCode);
        }


        #endregion
    }
}
