using ASPNetCoreJWTSample.Entities;
using System.Collections.Generic;
using System.Linq;
using Toolfactory.Core.Authorization;

namespace ASPNetCoreJWTSample.Services
{
    public class UsersRepository : IAuthorizationRepository
    {
        private readonly IEnumerable<User> _users;

        public UsersRepository(AccountService accountService)
        {
            _users = accountService.GetUsers();
        }

        public IEnumerable<string> GetRoles(string user)
        {
            return _users.Where(u => u.Username == user).Select(u => u.Role);
        }
    }
}
