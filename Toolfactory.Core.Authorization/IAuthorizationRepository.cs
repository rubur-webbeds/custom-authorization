using System;
using System.Collections.Generic;
using System.Text;

namespace Toolfactory.Core.Authorization
{
    public interface IAuthorizationRepository
    {
        IEnumerable<string> GetRoles(string user);
    }
}
