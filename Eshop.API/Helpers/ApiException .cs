namespace Eshop.API.Helpers
{
    public class ApiException : ResponseAPI
    {
        public string Details { get; set; }

        public ApiException(int statusCode, string? message = null, string details=null) : base(statusCode, message)
        {
            Details = details;
        }
    }
}
