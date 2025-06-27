using Eshop.Core.Entities.Auth;
using System.Dynamic;

namespace Eshop.Core.Services
{
    public interface IGenerateToken
    {
       string GetAndGenrateToken(AppUser user);
    }
}
