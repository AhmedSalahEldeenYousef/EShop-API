using Eshop.Core.DTO;

namespace Eshop.Core.Interfaces
{
    public interface IAuth
    {
         Task<string> RegisterAsync(RegisterDto registerDto);
        Task<string> LoginAsync(LoginDto loginDto);
        Task<bool> SendEmailForForgetPassword(string email);
        Task<string> ResetPasswordAsync(RestPasswordDto restPasswordDto);
        Task<bool> AcctiveAccountAsync(AcctiveAccountDto acctiveAccountDto);
        
    }
}
