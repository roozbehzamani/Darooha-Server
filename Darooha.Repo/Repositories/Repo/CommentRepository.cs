using Darooha.Data.DatabaseContext;
using Darooha.Data.Models;
using Darooha.Repo.Infrastructure;
using Darooha.Repo.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace Darooha.Repo.Repositories.Repo
{
    public class CommentRepository : Repository<Tbl_Comment>, ICommentRepository
    {
        private readonly DbContext _db;
        public CommentRepository(DbContext dbContext) : base(dbContext)
        {
            _db ??= (DaroohaDbContext)_db;
        }

    }
}