using System.Net;
using System.Text.Json;
using api.errors;

namespace api.Middleware;

public class Exceptionmiddleware ( RequestDelegate next , ILogger<Exceptionmiddleware> logger , 
IHostEnvironment env)
{
    public async Task InvokeAsync(HttpContext context)
    {   
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex , ex.Message);
            context.Response.ContentType="application/json";
            context.Response.StatusCode=(int)HttpStatusCode.InternalServerError;
            var response = env.IsDevelopment()
            ?new ApiExceptions(context.Response.StatusCode , ex.Message  , ex.StackTrace) 
            :new ApiExceptions(context.Response.StatusCode , ex.Message  , "internal server error");

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy=JsonNamingPolicy.CamelCase
            };
            var json = JsonSerializer.Serialize(response , options);
            await context.Response.WriteAsync(json);
        }

    }  
}
