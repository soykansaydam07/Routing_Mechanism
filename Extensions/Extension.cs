using Microsoft.AspNetCore.Builder;
using Routing_Mechanism.Middlewares;
using System.Runtime.CompilerServices;

namespace Routing_Mechanism.Extensions
{
    static public class Extension
    {
        public static IApplicationBuilder UseHello(this IApplicationBuilder applicationBuilder) 
        {
            return applicationBuilder.UseMiddleware<HelloMiddleware>();
        }
    }
}
