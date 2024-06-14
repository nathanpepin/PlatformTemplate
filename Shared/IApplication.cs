using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace Shared;

public interface IApplication
{
    static abstract WebApplicationBuilder Register(WebApplicationBuilder builder);
}