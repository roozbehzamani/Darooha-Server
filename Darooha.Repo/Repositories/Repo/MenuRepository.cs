﻿using Darooha.Data.DatabaseContext;
using Darooha.Data.Models;
using Darooha.Repo.Infrastructure;
using Darooha.Repo.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace Darooha.Repo.Repositories.Repo
{
    public class MenuRepository : Repository<Tbl_Menu>, IMenuRepository
    {
        private readonly DbContext _db;
        public MenuRepository(DbContext dbContext) : base(dbContext)
        {
            _db ??= (DaroohaDbContext)_db;
        }

    }
}
