using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizeApi.Repository
{
    public interface IAuthenticationManager
    {
        string Authenticate(string username, string password);
    }
}
