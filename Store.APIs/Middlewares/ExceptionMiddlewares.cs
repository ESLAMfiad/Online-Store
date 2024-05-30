using Store.APIs.Errors;
using System.Text.Json;

namespace Store.APIs.Middlewares
{
    public class ExceptionMiddlewares
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddlewares> _logger;
        private readonly IHostEnvironment _environment;

        public ExceptionMiddlewares(RequestDelegate Next,ILogger<ExceptionMiddlewares> logger,IHostEnvironment environment)
        {
            _next = Next;
            this._logger = logger;
            this._environment = environment;
        }
        //invoke async
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
               await _next.Invoke(context);

            }catch (Exception ex)
            {
                _logger.LogError(ex,ex.Message);
                //production = log ex in databse
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = 500; //aw (int) httpstatuscode.internalservererror; el etnen byrg3o 500
                //el if elgya 3shan tzhrly el server error f7alt bs ene development 
                //if(_environment.IsDevelopment())
                //{
                //    var Respone = new ApiExceptionResponse(500, ex.Message, ex.StackTrace.ToString());
                //}
                //else
                //{
                //    var Response = new ApiExceptionResponse(500);
                //}
                //syntax sugar : 
                var Response= _environment.IsDevelopment() ? new ApiExceptionResponse(500, ex.Message, ex.StackTrace.ToString()): new ApiExceptionResponse(500);
                var Options = new JsonSerializerOptions(){
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };
                var JsonResponse = JsonSerializer.Serialize(Response);
                await context.Response.WriteAsync(JsonResponse); //hna dft await 3shan elwarning
            }
        }
    }
}
