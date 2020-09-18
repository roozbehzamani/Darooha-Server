using Darooha.Data.DatabaseContext;
using Darooha.Repo.Infrastructure;
using Darooha.Data.Models;
using Darooha.Repo.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Darooha.Common.Helpers;

namespace Darooha.Repo.Repositories.Repo
{
    public class UserRepository : Repository<Tbl_User>, IUserRepository
    {
        private readonly DbContext _db;
        public UserRepository(DbContext dbContext) : base(dbContext)
        {
            _db ??= (DaroohaDbContext)_db;
        }

        public async Task<bool> UserExists(string Username, string UserType)
        {
            var user = await GetAsync(p => p.MobPhone.Equals(Username));
            if (user != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
