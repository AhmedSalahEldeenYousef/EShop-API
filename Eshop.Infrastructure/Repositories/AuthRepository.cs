using Eshop.Core.DTO;
using Eshop.Core.Entities.Auth;
using Eshop.Core.Interfaces;
using Eshop.Core.Services;
using Eshop.Core.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace Eshop.Infrastructure.Repositories
{
    public class AuthRepository : IAuth
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly SignInManager<AppUser> _signInManager;
        //private readonly IConfiguration _configuration;
        private readonly IGenerateToken _generateToken;
        public AuthRepository(UserManager<AppUser> userManager, IEmailService emailService, SignInManager<AppUser> signInManager, IGenerateToken token)
        {
            _userManager = userManager;
            _emailService = emailService;
            _signInManager = signInManager;
            //_configuration = configuration;
            _generateToken = token;
        }


        public async Task<string> RegisterAsync(RegisterDto registerDto)
        {

            if (registerDto is null)
            {
                return null;
            }
            if (await _userManager.FindByNameAsync(registerDto.UserName) is not null)
            {
                return "this user name is already registerd";
            }

            if (await _userManager.FindByEmailAsync(registerDto.UserName) is not null)
            {
                return "this user email is already registerd";
            }
            AppUser user = new AppUser
            {
                UserName = registerDto.UserName,
                Email = registerDto.Email,
                DisplayName = registerDto.DisplayName,
            };

            var addUserResult = await _userManager.CreateAsync(user, registerDto.Password);
            if (!addUserResult.Succeeded)
            {
                //return addUserResult.Errors.ToList()[0].Description;
                //OR
                return string.Join(", ", addUserResult.Errors.Select(e => e.Description));

            }

            //Sending active email
            //await SendEmail(user.Email, await _userManager.GenerateEmailConfirmationTokenAsync(user), "active", "Active your account", "Click here to active your account");

            string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            await SendEmail(user.Email, token, "active", "ActiveEmail", "Please active your email, click on button to active");

            //await _userManager.AddToRoleAsync(user, "Customer");
            return "User registered successfully";

        }


        public async Task<string> LoginAsync(LoginDto loginDto)
        {
            if (loginDto is null)
            {
                return null;
            }
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user is null)
            {
                return "this email is not registered";
            }
            if (!user.EmailConfirmed)
            {
                string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                await SendEmail(user.Email, await _userManager.GenerateEmailConfirmationTokenAsync(user), "active", "Please Active your account", "Click here to active your account");
                return "Please active your account, we sent you an email to active your account";

            }

            var result = await _signInManager.PasswordSignInAsync(user, loginDto.Password, false, true);
            if (!result.Succeeded)
            {
                return "Please check : Wrong email or password";
            }
            return _generateToken.GetAndGenrateToken(user);
        }

        public async Task SendEmail(string email, string code, string component, string subject, string message)
        {
            var result = new EmailDto(email,"a.salaheldeen21993@gmail.com",subject, EmailStringBody.Send(email, code, component, message));
            await _emailService.SendEmail(result);
        }


        //send email for For Forget password
        public async Task<bool> SendEmailForForgetPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return false;
            }
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return false; // User not found
            }
            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            await SendEmail(user.Email, token, "reset-password", "Reset your password", "Click here to reset your password");
            return true; // Email sent successfully
        }

        //send email for reset password
        public async Task<string> ResetPasswordAsync(RestPasswordDto resetPasswordDto)
        {
            if (resetPasswordDto is null)
            {
                return null;
            }
            var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);
            if (user is null)
            {
                return "this email is not registered";
            }
            var resetResult = await _userManager.ResetPasswordAsync(user, resetPasswordDto.Token, resetPasswordDto.Password);

            if (!resetResult.Succeeded)
            {
                return string.Join(", ", resetResult.Errors.Select(e => e.Description));
            }

            //string token = await _userManager.GeneratePasswordResetTokenAsync(user);
           // await SendEmail(user.Email, token, "reset-password", "Reset your password", "Click here to reset your password");
            return "We sent you an email to reset your password";
        }

        //active user account
        public async Task<bool> AcctiveAccountAsync(AcctiveAccountDto acctiveAccountDto)
        {
            if (acctiveAccountDto is null)
            {
                return false;
            }
            var user = await _userManager.FindByEmailAsync(acctiveAccountDto.Email);
            if (user == null)
            {
                return false; // User not found
            }
            var result = await _userManager.ConfirmEmailAsync(user, acctiveAccountDto.Token);

            if (result.Succeeded)
            {
                return true; // Return true if email confirmation succeeded
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            await SendEmail(user.Email, token, "active", "Active your account", "Click here to active your account");
            return false; // Return false if email confirmation failed, and resend the activation email
        }


    }
}