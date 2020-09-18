using Darooha.Data.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;


namespace Darooha.Test.UnitTests.Providers
{
    public class FakeSignInManager : SignInManager<Tbl_User>
    {
        public FakeSignInManager()
            : base(new Mock<FakeUserManager>().Object,
                new Mock<IHttpContextAccessor>().Object,
                new Mock<IUserClaimsPrincipalFactory<Tbl_User>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<ILogger<SignInManager<Tbl_User>>>().Object,
                new Mock<IAuthenticationSchemeProvider>().Object,
                new Mock<IUserConfirmation<Tbl_User>>().Object
            )
        { }
    }
}
