using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Movies.Api.Auth;

public class ApiKeyAuthFilter : IAuthorizationFilter
{
    private readonly IConfiguration _configuration;

    public ApiKeyAuthFilter(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        const string apiKeyMissingMessage = "API Key is missing";
        if (!context.HttpContext.Request.Headers.TryGetValue(AuthConstants.ApiKeyHeaderName,
                 out var extractedApiKey))
        {
            context.Result = new UnauthorizedObjectResult(apiKeyMissingMessage);
            return;
        }
        
        var validKey = _configuration["ApiKey"];
        if (validKey != extractedApiKey)
        {
            context.Result = new UnauthorizedObjectResult(apiKeyMissingMessage);
        }
    }
}