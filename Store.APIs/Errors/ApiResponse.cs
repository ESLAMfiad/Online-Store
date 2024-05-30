
namespace Store.APIs.Errors
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public ApiResponse(int statusCode,string? message=null)
        {
                StatusCode = statusCode;
                Message = message ?? GetDefaultMessageForStatusCode(StatusCode);

        }

        private string? GetDefaultMessageForStatusCode(int? statusCode)
        {
            //500 = internal server error
            //400 = bad req
            //401 = unauthorized
            //404= not found

            //c# 7 feature gdeda
            return StatusCode switch
            {
                400 => "Bad Request",
                401 => "you are not authorized",
                404 => "not found",
                500 => "internal server error",
                _ => null //else
            };
        }
    }
}
