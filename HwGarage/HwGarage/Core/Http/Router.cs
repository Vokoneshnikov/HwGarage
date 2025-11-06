using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HwGarage.Core.Http
{
    public class Router
    {
        private readonly Dictionary<string, Func<HttpContext, Task>> _routes = new();

        public void Register(string path, Func<HttpContext, Task> handler)
        {
            _routes[path] = handler;
        }

        public async Task RouteAsync(HttpContext context)
        {
            var requestPath = context.Request.Url!.AbsolutePath.TrimEnd('/').ToLower();

            if (_routes.TryGetValue(requestPath, out var handler))
            {
                await handler(context);
            }
            else
            {
                context.WriteResponse("<h1>404 - Not Found</h1>", "text/html", 404);
            }
        }
    }
}