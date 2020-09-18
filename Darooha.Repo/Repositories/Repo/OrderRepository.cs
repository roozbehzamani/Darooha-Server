using Darooha.Data.DatabaseContext;
using Darooha.Data.Models;
using Darooha.Repo.Infrastructure;
using Darooha.Repo.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace Darooha.Repo.Repositories.Repo
{
    public class OrderRepository : Repository<Tbl_Order>, IOrderRepository
    {
        private readonly DbContext _db;
        public OrderRepository(DbContext dbContext) : base(dbContext)
        {
            _db ??= (DaroohaDbContext)_db;
        }

    }
}
