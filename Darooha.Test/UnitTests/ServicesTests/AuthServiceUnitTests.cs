using Darooha.Common.Helpers.Interface;
using Darooha.Data.DatabaseContext;
using Darooha.Data.Models;
using Darooha.Repo.Infrastructure;
using Darooha.Services.Site.Admin.Auth.Service;
using Darooha.Test.DataInput;
using Moq;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace Darooha.Test.UnitTests.ServicesTests
{
    public class AuthServiceUnitTests
    {
        private readonly Mock<IUnitOfWork<DaroohaDbContext>> _mockRepo;
        private readonly Mock<IUtilities> _mockUtilities;
        private readonly AuthService _service;

        public AuthServiceUnitTests()
        {
            _mockRepo = new Mock<IUnitOfWork<DaroohaDbContext>>();
            _mockUtilities = new Mock<IUtilities>();
            _service = new AuthService(_mockRepo.Object, _mockUtilities.Object);

        }

        #region LoginTests
        [Fact]
        public async Task Login_Success()
        {
            //Arrange------------------------------------------------------------------------------------------------------------------------------
            _mockRepo.Setup(x => x.UserRepository.GetAsync(It.IsAny<Expression<Func<Tbl_User, bool>>>())).ReturnsAsync(UnitTestsDataInput.GetUser.First());

            _mockUtilities.Setup(x => x.VerifyPasswordHash(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<byte[]>()))
                .Returns(true);
            //Act----------------------------------------------------------------------------------------------------------------------------------
            var result = await _service.LoginAsync(It.IsAny<string>(), It.IsAny<string>());

            //Assert-------------------------------------------------------------------------------------------------------------------------------
            Assert.NotNull(result);
            Assert.IsType<Tbl_User>(result);

        }
        [Fact]
        public async Task Login_Fail_WrongUserName()
        {
            //Arrange------------------------------------------------------------------------------------------------------------------------------
            _mockRepo.Setup(x => x.UserRepository
                .GetManyAsync(
                    It.IsAny<Expression<Func<Tbl_User, bool>>>(),
                    It.IsAny<Func<IQueryable<Tbl_User>, IOrderedQueryable<Tbl_User>>>(),
                    It.IsAny<string>())).ReturnsAsync(Enumerable.Empty<Tbl_User>());

            //Act----------------------------------------------------------------------------------------------------------------------------------
            var result = await _service.LoginAsync(It.IsAny<string>(), It.IsAny<string>());

            //Assert-------------------------------------------------------------------------------------------------------------------------------
            Assert.Null(result);
        }
        [Fact]
        public async Task Login_Fail_WrongPassWord()
        {
            //Arrange------------------------------------------------------------------------------------------------------------------------------
            _mockRepo.Setup(x => x.UserRepository
                .GetManyAsync(
                    It.IsAny<Expression<Func<Tbl_User, bool>>>(),
                    It.IsAny<Func<IQueryable<Tbl_User>, IOrderedQueryable<Tbl_User>>>(),
                    It.IsAny<string>())).ReturnsAsync(UnitTestsDataInput.GetUser);

            _mockUtilities.Setup(x => x.VerifyPasswordHash(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<byte[]>()))
                .Returns(false);
            //Act----------------------------------------------------------------------------------------------------------------------------------
            var result = await _service.LoginAsync(It.IsAny<string>(), It.IsAny<string>());

            //Assert-------------------------------------------------------------------------------------------------------------------------------
            Assert.Null(result);

        }
        #endregion

        #region RegisterTests
        [Fact]
        public async Task Register_Success()
        {
            //Arrange------------------------------------------------------------------------------------------------------------------------------
            byte[] passwordHash, passwordSalt;
            _mockUtilities.Setup(x => x.CreatePasswordHash(It.IsAny<string>(),
                out passwordHash, out passwordSalt));

            _mockRepo.Setup(x => x.UserRepository.InsertAsync(It.IsAny<Tbl_User>()));

            _mockRepo.Setup(x => x.SaveAsync()).ReturnsAsync(true);

            //Act----------------------------------------------------------------------------------------------------------------------------------
            var result = await _service.RegisterAsync(new Tbl_User());

            //Assert-------------------------------------------------------------------------------------------------------------------------------
            Assert.NotNull(result);
            Assert.IsType<Tbl_User>(result);

        }
        [Fact]
        public async Task Register_Fail_dbError()
        {
            //Arrange------------------------------------------------------------------------------------------------------------------------------
            byte[] passwordHash, passwordSalt;
            _mockUtilities.Setup(x => x.CreatePasswordHash(It.IsAny<string>(),
                out passwordHash, out passwordSalt));

            _mockRepo.Setup(x => x.UserRepository.InsertAsync(It.IsAny<Tbl_User>()));

            _mockRepo.Setup(x => x.SaveAsync()).ReturnsAsync(false);

            //Act----------------------------------------------------------------------------------------------------------------------------------
            var result = await _service.RegisterAsync(new Tbl_User());

            //Assert-------------------------------------------------------------------------------------------------------------------------------
            Assert.Null(result);

        }
        #endregion
    }
}
