using System.Net;
using System.Text.Json;
using Eshop.API.Helpers;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;

namespace Eshop.API.middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _environment;
        private readonly IMemoryCache _memory;
        private readonly TimeSpan _reateLimitWindow = TimeSpan.FromSeconds(30);

        public ExceptionMiddleware(RequestDelegate next, IHostEnvironment environment, IMemoryCache memory)
        {
            _next = next;
            _environment = environment;
            _memory = memory;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Apply HTTP Security Headers for Browser Hardening
                ApplySecurity(context);

                if (IsRequestAllowed(context) == false)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
                    context.Response.ContentType = "application/json";
                    var Response = new ApiException((int)HttpStatusCode.TooManyRequests, "Too many requests: please try again later");
                    await context.Response.WriteAsJsonAsync(Response);
                }
                await _next(context);
            }
            catch (Exception ex)
            {

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var response = _environment.IsDevelopment() ?
                      new ApiException((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace!)
                    : new ApiException((int)HttpStatusCode.InternalServerError, ex.Message);

                var json = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(json);
            }
        }

        //handle rate limiting 
        private bool IsRequestAllowed(HttpContext context)
        {
            //get id address
            if (context.Connection.RemoteIpAddress == null)
            {
                return false;

            }
            var ip = context.Connection.RemoteIpAddress.ToString();
            var ChachKey = $"Reate: {ip}";
            var DateNow = DateTime.Now;
            //using memory cache
            var (timestamp, count) = _memory.GetOrCreate(ChachKey, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = _reateLimitWindow;
                return (timestamp: DateNow, count:0);
            });
            if (DateNow - timestamp < _reateLimitWindow)
            {
                if (count >= 30) //8s
                {
                    return false;
                }
                _memory.Set(ChachKey, (timestamp, count += 1), _reateLimitWindow);
            }
            else
            {
                _memory.Set(ChachKey, (timestamp, count), _reateLimitWindow);
            }
            return true;
        }

        private void ApplySecurity(HttpContext context)
        {
            context.Response.Headers["X-Content-Options"] = "nosniff";
            context.Response.Headers["X-xss-Protection"] = "1; mode=block";
            context.Response.Headers["X-Frame-Options"] = "DENY";
            //
            context.Response.Headers["Strict-Transport-Security"] = "max-age=63072000; includeSubDomains; preload";
            context.Response.Headers["Content-Security-Policy"] = "default-src 'self'";
            context.Response.Headers["Referrer-Policy"] = "strict-origin-when-cross-origin";
            context.Response.Headers["Permissions-Policy"] = "geolocation=(), microphone=(), camera=()";
            context.Response.Headers["Cross-Origin-Embedder-Policy"] = "require-corp";
            context.Response.Headers["Cross-Origin-Opener-Policy"] = "same-origin";
            context.Response.Headers["X-Download-Options"] = "noopen";
        }

    }

}
