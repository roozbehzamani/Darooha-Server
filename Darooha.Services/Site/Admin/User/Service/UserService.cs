using Darooha.Common.Helpers;
using Darooha.Common.Helpers.Interface;
using Darooha.Data.DatabaseContext;
using Darooha.Data.Models;
using Darooha.Repo.Infrastructure;
using Darooha.Services.Site.Admin.User.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Darooha.Services.Site.Admin.User.Service
{
    public class UserService : IUserService
    {

        private readonly IUnitOfWork<DaroohaDbContext> _db;
        private readonly IUtilities _utilities;

        public UserService(IUnitOfWork<DaroohaDbContext> dbContext, IUtilities utilities)
        {
            _db = dbContext;
            _utilities = utilities;
        }

        public async Task<Tbl_User> GetUserForPassChange(string id, string Password)
        {
            var user = await _db.UserRepository.GetByIdAsync(id);

            if (user == null)
                return null;


            return user;
        }

        public async Task<bool> UpdateUserPassword(Tbl_User user, string NewPassword)
        {
            byte[] PasswordHash, PasswordSalt;
            _utilities.CreatePasswordHash(NewPassword, out PasswordHash, out PasswordSalt);

            //user.PasswordHash = PasswordHash;
            //user.PasswordSalt = PasswordSalt;

            _db.UserRepository.Update(user);
            return await _db.SaveAsync();
        }
    }
}
