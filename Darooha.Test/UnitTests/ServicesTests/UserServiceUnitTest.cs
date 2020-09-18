using Darooha.Common.Helpers.Interface;
using Darooha.Data.DatabaseContext;
using Darooha.Data.Models;
using Darooha.Repo.Infrastructure;
using Darooha.Services.Site.Admin.User.Service;
using Darooha.Test.DataInput;
using Moq;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Darooha.Test.UnitTests.ServicesTests
{
    public class UserServiceUnitTest
    {
        private readonly Mock<IUnitOfWork<DaroohaDbContext>> _mockRepo;
        private readonly Mock<IUtilities> _mockUtilities;
        private readonly UserService _service;

        public UserServiceUnitTest()
        {
            _mockRepo = new Mock<IUnitOfWork<DaroohaDbContext>>();
            _mockUtilities = new Mock<IUtilities>();
            _service = new UserService(_mockRepo.Object, _mockUtilities.Object);

        }

        #region GetUserForPassChangeTests
        [Fact]
        public async Task GetUserForPassChange_Success()
        {
            //Arrange------------------------------------------------------------------------------------------------------------------------------
            _mockRepo.Setup(x => x.UserRepository.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(UnitTestsDataInput.GetUser.First());

            _mockUtilities.Setup(x => x.VerifyPasswordHash(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<byte[]>()))
                .Returns(true);


            //Act----------------------------------------------------------------------------------------------------------------------------------
            var result = await _service.GetUserForPassChange(It.IsAny<string>(), It.IsAny<string>());

            //Assert-------------------------------------------------------------------------------------------------------------------------------
            Assert.NotNull(result);
            Assert.IsType<Tbl_User>(result);

        }
        [Fact]
        public async Task GetUserForPassChange_Fail_WrongId()
        {
            //Arrange------------------------------------------------------------------------------------------------------------------------------
            _mockRepo.Setup(x => x.UserRepository.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(It.IsAny<Tbl_User>());


            //Act----------------------------------------------------------------------------------------------------------------------------------
            var result = await _service.GetUserForPassChange(It.IsAny<string>(), It.IsAny<string>());

            //Assert-------------------------------------------------------------------------------------------------------------------------------
            Assert.Null(result);

        }
        [Fact]
        public async Task GetUserForPassChange_Fail_WrongPassword()
        {
            //Arrange------------------------------------------------------------------------------------------------------------------------------
            _mockRepo.Setup(x => x.UserRepository.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new Tbl_User());

            //Act----------------------------------------------------------------------------------------------------------------------------------
            var result = await _service.GetUserForPassChange(It.IsAny<string>(), It.IsAny<string>());

            //Assert-------------------------------------------------------------------------------------------------------------------------------
            Assert.Null(result.Email);

        }
        #endregion


        #region UpdateUserPassTests
        [Fact]
        public async Task UpdateUserPass_Success()
        {
            //Arrange------------------------------------------------------------------------------------------------------------------------------

            byte[] passwordHash, passwordSalt;

            _mockUtilities.Setup(x => x.CreatePasswordHash(It.IsAny<string>(), out passwordHash, out passwordSalt));

            _mockRepo.Setup(x => x.UserRepository.Update(It.IsAny<Tbl_User>()));

            _mockRepo.Setup(x => x.SaveAsync()).ReturnsAsync(true);



            //Act----------------------------------------------------------------------------------------------------------------------------------
            var result = await _service.UpdateUserPassword(new Tbl_User(), It.IsAny<string>());

            //Assert-------------------------------------------------------------------------------------------------------------------------------
            Assert.True(result);

        }
        [Fact]
        public async Task UpdateUserPass_Fail_dbError()
        {
            //Arrange------------------------------------------------------------------------------------------------------------------------------

            byte[] passwordHash, passwordSalt;

            _mockUtilities.Setup(x => x.CreatePasswordHash(It.IsAny<string>(), out passwordHash, out passwordSalt));

            _mockRepo.Setup(x => x.UserRepository.Update(It.IsAny<Tbl_User>()));

            _mockRepo.Setup(x => x.SaveAsync()).ReturnsAsync(false);



            //Act----------------------------------------------------------------------------------------------------------------------------------
            var result = await _service.UpdateUserPassword(new Tbl_User(), It.IsAny<string>());

            //Assert-------------------------------------------------------------------------------------------------------------------------------
            Assert.False(result);

        }
        #endregion
    }
}
