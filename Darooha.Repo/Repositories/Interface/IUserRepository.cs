using Darooha.Repo.Infrastructure;
using Darooha.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Darooha.Repo.Repositories.Interface
{
    public interface IUserRepository : IRepository<Tbl_User>
    {
        Task<bool> UserExists(string Username, string UserType);
    }
}
