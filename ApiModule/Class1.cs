using Microsoft.AspNetCore.Builder;
using Shared;

namespace ApiModule;

public class ApiModule : IApplication
{
    public static WebApplicationBuilder Register(WebApplicationBuilder builder)
    {
        return builder;
    }
}