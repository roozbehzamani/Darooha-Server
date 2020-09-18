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
    public class SettingRepository : Repository<Tbl_Setting>, ISettingRepository
    {
        private readonly DbContext _db;
        public SettingRepository(DbContext dbContext) : base(dbContext)
        {
            _db ??= (DaroohaDbContext)_db;
        }
    }
}
