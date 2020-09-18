using Darooha.Data.DatabaseContext;
using Darooha.Data.Models;
using Darooha.Repo.Infrastructure;
using Darooha.Repo.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Darooha.Repo.Repositories.Repo
{
    public class UserAddressRepository : Repository<Tbl_UserAddress>, IUserAddressRepository
    {
        private readonly DbContext _db;
        public UserAddressRepository(DbContext dbContext) : base(dbContext)
        {
            _db ??= (DaroohaDbContext)_db;
        }
    }
}
