using Darooha.Data.DatabaseContext;
using Darooha.Data.Models;
using Darooha.Repo.Infrastructure;
using Darooha.Repo.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace Darooha.Repo.Repositories.Repo
{
    public class ProductRepository : Repository<Tbl_Product>, IProductRepository
    {
        private readonly DbContext _db;
        public ProductRepository(DbContext dbContext) : base(dbContext)
        {
            _db ??= (DaroohaDbContext)_db;
        }

    }
}