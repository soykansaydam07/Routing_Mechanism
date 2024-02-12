using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Routing_Mechanism.Middlewares
{
    public class HelloMiddleware
    {
        RequestDelegate _next;
        public HelloMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext httpContext)
        {
            //Custom Operasyon

            Console.WriteLine("Hello");
            await _next.Invoke(httpContext);

            Console.WriteLine("GoodBye");
        }
    }
}
