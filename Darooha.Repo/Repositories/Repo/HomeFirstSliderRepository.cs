using Darooha.Data.DatabaseContext;
using Darooha.Data.Models;
using Darooha.Repo.Infrastructure;
using Darooha.Repo.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace Darooha.Repo.Repositories.Repo
{
    public class HomeFirstSliderRepository : Repository<Tbl_HomeFirstSlider>, IHomeFirstSliderRepository
    {
        private readonly DbContext _db;
        public HomeFirstSliderRepository(DbContext dbContext) : base(dbContext)
        {
            _db ??= (DaroohaDbContext)_db;
        }

    }
}
