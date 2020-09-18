using System;
using Darooha.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace Darooha.Test.UnitTests.Providers
{
    public class FakeUserManager : UserManager<Tbl_User>
    {
        public FakeUserManager()
            : base(new Mock<IUserStore<Tbl_User>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<Tbl_User>>().Object,
                new IUserValidator<Tbl_User>[0],
                new IPasswordValidator<Tbl_User>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<Tbl_User>>>().Object)
        {



        }
    }
}
