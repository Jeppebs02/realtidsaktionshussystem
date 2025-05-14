using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System;

public class ApiKeyAuthMiddleware
{
    private readonly RequestDelegate _next;
    private const string API_KEY_HEADER = "x-api-key"; // KEY part of key:value pair
    private readonly string _expectedApiKey; // Value part of key:value pair


    // A request delicate represents the NEXT step in the HTTP request pipeline
    // We dont have to point to this step ourselves, ASP:NET Core does that for us.
    // What is important, is that a RequestDelegate takes a HttpContext object.

    // Think of it like how Task represents a unit of work in async programming
    public ApiKeyAuthMiddleware(RequestDelegate next)
    {
        _next = next;
        _expectedApiKey = Environment.GetEnvironmentVariable("api-key").Trim();


        if (string.IsNullOrWhiteSpace(_expectedApiKey))
        {
            // if no api key, give error
            Console.WriteLine("CRITICAL ERROR: API Key ('ApiKey' in config or YOUR_ENV_VARIABLE_FOR_API_KEY) is not configured on the server!");
            // create random key, so we are forced to stop the program and set the key
            _expectedApiKey = Guid.NewGuid().ToString();
        }
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // The http context represents the current HTTP request. That includes headers, body, etc.
        // Think of this method as "doing calculations" on the request.

        if (!context.Request.Headers.TryGetValue(API_KEY_HEADER, out var requestApiKeyHeaderValue))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized; // status code can be whatever we want
            await context.Response.WriteAsync("API Key was not provided :(");
            return;
        }

        string requestApiKey = requestApiKeyHeaderValue.ToString();

        if (!_expectedApiKey.Equals(requestApiKey))
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden; // same as above
            await context.Response.WriteAsync("Invalid API Key :(");
            return;
        }

        //Now that we are done with our "calculations" we can pass the HttpContext to the next step in the pipeline.
        // (Because our _next is a RequestDelegate, it takes a HttpContext as a parameter)

        // API key is valid, proceed to the endpoint
        await _next(context);
    }
}