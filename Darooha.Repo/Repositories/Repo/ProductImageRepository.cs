using Darooha.Data.DatabaseContext;
using Darooha.Data.Models;
using Darooha.Repo.Infrastructure;
using Darooha.Repo.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace Darooha.Repo.Repositories.Repo
{
    public class ProductImageRepository : Repository<Tbl_ProductImage>, IProductImageRepository
    {
        private readonly DbContext _db;
        public ProductImageRepository(DbContext dbContext) : base(dbContext)
        {
            _db ??= (DaroohaDbContext)_db;
        }

    }
}