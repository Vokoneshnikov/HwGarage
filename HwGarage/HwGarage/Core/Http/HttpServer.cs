using System;
using System.Net;
using System.Threading.Tasks;
using HwGarage.Core.Http.Middleware;

namespace HwGarage.Core.Http
{
    public class HttpServer
    {
        private readonly HttpListener _listener;
        private readonly BaseMiddleware _middlewarePipeline;

        public HttpServer(string prefix, BaseMiddleware middlewarePipeline)
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add(prefix);
            _middlewarePipeline = middlewarePipeline;
        }

        public async Task StartAsync()
        {
            _listener.Start();
            Console.WriteLine($"[SERVER] Listening on {_listener.Prefixes.First()}");

            while (_listener.IsListening)
            {
                var context = await _listener.GetContextAsync();
                var httpContext = new HttpContext(context);

                try
                {
                    await _middlewarePipeline.InvokeAsync(httpContext);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ERROR] {ex.Message}");
                    httpContext.WriteResponse("<h1>500 - Internal Server Error</h1>", "text/html", 500);
                }
                finally
                {
                    context.Response.OutputStream.Close();
                }
            }
        }

        public void Stop()
        {
            _listener.Stop();
            Console.WriteLine("[SERVER] Stopped.");
        }
    }
}