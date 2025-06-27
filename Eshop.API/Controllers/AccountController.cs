using AutoMapper;
using Eshop.API.Helpers;
using Eshop.Core.DTO;
using Eshop.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Eshop.API.Controllers
{

    public class AccountController : BaseController
    {
        public AccountController(IUnitOfWork work, IMapper mapper) : base(work, mapper)
        {
        }

        [HttpPost("Register")]
        //public async Task<ActionResult> Register([FromBody] RegisterDto registerDto)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var result = await _work.Auth.RegisterAsync(registerDto);
        //    if (result != "User registered successfully")
        //    {
        //        return BadRequest(new ResponseAPI(400,result));
        //    }
        //    else
        //    {
        //        return Ok(new ResponseAPI(200, result));
        //    }
        //}

        public async Task<ActionResult> Register(RegisterDto registerDto)
        {
            var result = await _work.Auth.RegisterAsync(registerDto);
            if (result != "User registered successfully")
            {
                return BadRequest(new ResponseAPI(400, result));
            }
            else
            {
                return Ok(new ResponseAPI(200, result));
            }
        }
        [HttpPost("Login")]
        public async Task<ActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _work.Auth.LoginAsync(loginDto);
            if (result == null)
            {
                return Unauthorized(new ResponseAPI(401, "Invalid username or password"));
            }
            else if (result.StartsWith("Please"))
            {
                return BadRequest(new ResponseAPI(400, result));
            }

            else
            {
                Response.Cookies.Append("token", result, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict, // Prevent CSRF attacks by ensuring the cookie is sent only with same-site requests
                    IsEssential = true, // Ensure the cookie is sent with requests
                    Domain = "localhost",
                    Expires = DateTimeOffset.UtcNow.AddDays(7) // Set cookie expiration to 7 days
                });
                return Ok(new ResponseAPI(200, result));
            }
        }

        [HttpPost("active-account")]
        public async Task<ActionResult> Active(AcctiveAccountDto acctiveAccountDto)

        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _work.Auth.AcctiveAccountAsync(acctiveAccountDto);
            // Check if the result is false, indicating an error in activation
            return result
                ? Ok(new ResponseAPI(200, "Account activated successfully"))
                : BadRequest(new ResponseAPI(400, "Failed to activate account. Please check your email for the activation link."));
        }

        [HttpGet("send-email-forget-password")]
        public async Task<ActionResult> ForgetPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest(new ResponseAPI(400, "Email is required"));
            }
            var result = await _work.Auth.SendEmailForForgetPassword(email);
       return result
                ? Ok(new ResponseAPI(200, "Email sent successfully"))
                : BadRequest(new ResponseAPI(400, "Failed to send email. Please check the email address and try again."));
        }
    }
}
