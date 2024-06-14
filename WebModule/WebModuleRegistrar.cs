using Microsoft.AspNetCore.Builder;
using Shared;

namespace WebModule;

public class WebModuleRegistrar : IApplication
{
    public static WebApplicationBuilder Register(WebApplicationBuilder builder)
    {
        return builder;
    }
}