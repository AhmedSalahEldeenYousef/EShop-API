namespace Eshop.API.Helpers
{
    public class ResponseAPI
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public ResponseAPI(int statusCode, string? message =null)
        {
            StatusCode = statusCode;
            Message = message ?? GetMessageFromStatusCode(statusCode);
        }

        private string? GetMessageFromStatusCode(int statuscode)
        {
            return statuscode switch
            {
                200 => "Done Successfully",
                400=>  "Bad Request",
                401=>  "UnAuthorized",
                404=>  "Not Found Recources Api End Point Not Found",
                500 => "Server Error",
                _ => null,
            };
        }

    }
}
