using Darooha.Common.ErrorsAndMessages;
using Darooha.Data.Dtos.Site.Panel.User;
using Darooha.Presentation;
using Darooha.Test.DataInput;
using Darooha.Test.IntegrationTests.Providers;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xunit;

namespace Darooha.Test.IntegrationTests.ControllersTests
{
    public class UserControllerTests : IClassFixture<TestClientProvider<Startup>>
    {
        private HttpClient _client;
        public UserControllerTests(TestClientProvider<Startup> testClientProvider)
        {
            _client = testClientProvider.Client;
        }

        #region GetUserTests
        [Fact]
        public async Task GetUser_Failed_GetAnOtherUser()
        {
            // Arrange
            string anOtherUserId = UnitTestsDataInput.userAnOtherId;
            var request = "/site/panel/User/" + anOtherUserId;

            _client.DefaultRequestHeaders.Authorization
           = new AuthenticationHeaderValue("Bearer", UnitTestsDataInput.aToken);

            //Act
            var response = await _client.GetAsync(request);
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
        [Fact]
        public async Task GetUser_Success_GetUserHimself()
        {
             //api/v1/site/panel/User/{id}
            // Arrange
            string userHimSelfId = UnitTestsDataInput.currentUserId;
            var request = "/api/v1/site/panel/User/" + userHimSelfId;
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", UnitTestsDataInput.aToken);

            //Act
            var response = await _client.GetAsync(request);
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        #endregion

        #region UpdateUserTests
        [Fact]
        public async Task UpdateUser_Failed_UpdateAnOtherUser()
        {
            // Arrange
            string anOtherUserId = UnitTestsDataInput.userAnOtherId;
            var request = new
            {
                Url = "/site/panel/User/" + anOtherUserId,
                Body = UnitTestsDataInput.userForUpdateDto
            };
            _client.DefaultRequestHeaders.Authorization
           = new AuthenticationHeaderValue("Bearer", UnitTestsDataInput.aToken);

            //Act
            var response = await _client.PutAsync(request.Url, ContentHelper.GetStringContent(request.Body));

            //Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
        [Fact]
        public async Task UpdateUser_Success_UpdateUserHimself()
        {
            // Arrange
            string currentUserId = UnitTestsDataInput.currentUserId;
            var request = new
            {
                Url = "/site/panel/User/" + currentUserId,
                Body = UnitTestsDataInput.userForUpdateDto
            };
            _client.DefaultRequestHeaders.Authorization
           = new AuthenticationHeaderValue("Bearer", UnitTestsDataInput.aToken);

            //Act
            var response = await _client.PutAsync(request.Url, ContentHelper.GetStringContent(request.Body));

            //Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
        [Fact]
        public async Task UpdateUser_Failed_ModelStateError()
        {
            // Arrange
            string currentUserId = UnitTestsDataInput.currentUserId;
            var request = new
            {
                Url = "/site/panel/User/" + currentUserId,
                Body = UnitTestsDataInput.userForUpdateDto_Fail_ModelState
            };
            _client.DefaultRequestHeaders.Authorization
           = new AuthenticationHeaderValue("Bearer", UnitTestsDataInput.aToken);


            //Act
            var response = await _client.PutAsync(request.Url, ContentHelper.GetStringContent(request.Body));


            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        #endregion

        #region ChangeUserPasswordTests
        [Fact]
        public async Task ChangeUserPassword_Success_Himself()
        {
            //Arrange------------------------------------------------------------------------------------------------------------------------------
            string currentUserId = UnitTestsDataInput.currentUserId;
            var request = new
            {
                Url = "/site/panel/User/ChangeUserPassword/" + currentUserId,
                Body = UnitTestsDataInput.passwordForChangeDto
            };
            _client.DefaultRequestHeaders.Authorization
           = new AuthenticationHeaderValue("Bearer", UnitTestsDataInput.aToken);

            //Act----------------------------------------------------------------------------------------------------------------------------------
            var response = await _client.PutAsync(request.Url, ContentHelper.GetStringContent(request.Body));

            //Assert-------------------------------------------------------------------------------------------------------------------------------
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
        [Fact]
        public async Task ChangeUserPassword_Failed_AnOtherUser()
        {
            //Arrange------------------------------------------------------------------------------------------------------------------------------
            string anOtherUserId = UnitTestsDataInput.userAnOtherId;
            var request = new
            {
                Url = "/site/panel/User/ChangeUserPassword/" + anOtherUserId,
                Body = UnitTestsDataInput.passwordForChangeDto
            };
            _client.DefaultRequestHeaders.Authorization
           = new AuthenticationHeaderValue("Bearer", UnitTestsDataInput.aToken);

            //Act----------------------------------------------------------------------------------------------------------------------------------
            var response = await _client.PutAsync(request.Url, ContentHelper.GetStringContent(request.Body));

            //Assert-------------------------------------------------------------------------------------------------------------------------------
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
        [Fact]
        public async Task ChangeUserPassword_Failed_Himself_WrongOldPassword()
        {
            //Arrange------------------------------------------------------------------------------------------------------------------------------
            string currentUserId = UnitTestsDataInput.currentUserId;
            var request = new
            {
                Url = "/site/panel/User/ChangeUserPassword/" + currentUserId,
                Body = new PasswordForChangeDTO
                {
                    OldPassword = "123789654645",
                    NewPassword = "123789"
                }
            };
            _client.DefaultRequestHeaders.Authorization
           = new AuthenticationHeaderValue("Bearer", UnitTestsDataInput.aToken);

            //Act----------------------------------------------------------------------------------------------------------------------------------
            var response = await _client.PutAsync(request.Url, ContentHelper.GetStringContent(request.Body));
            var value = await response.Content.ReadAsStringAsync();
            var valueObj = JsonConvert.DeserializeObject<ReturnErrorMessage>(value);

            //Assert-------------------------------------------------------------------------------------------------------------------------------
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.False(valueObj.Status);
            Assert.Equal("پسورد فعلی اشتباه میباشد . لطفا مجددا تلاش نمایید", valueObj.Message);


        }
        [Fact]
        public async Task ChangeUserPassword_Failed_ModelStateError()
        {
            //Arrange------------------------------------------------------------------------------------------------------------------------------
            string currentUserId = UnitTestsDataInput.currentUserId;
            var request = new
            {
                Url = "/site/panel/User/ChangeUserPassword/" + currentUserId,
                Body = UnitTestsDataInput.passwordForChangeDto_Fail_ModelState
            };
            _client.DefaultRequestHeaders.Authorization
           = new AuthenticationHeaderValue("Bearer", UnitTestsDataInput.aToken);

            //Act----------------------------------------------------------------------------------------------------------------------------------
            var response = await _client.PutAsync(request.Url, ContentHelper.GetStringContent(request.Body));


            //Assert-------------------------------------------------------------------------------------------------------------------------------
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        }
        #endregion
    }
}