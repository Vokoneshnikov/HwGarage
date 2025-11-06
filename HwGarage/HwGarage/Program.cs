using HwGarage.Core.Http;
using HwGarage.Core.Http.Middleware;
using System.Threading.Tasks;

class SimpleMiddleware : BaseMiddleware
{
    public SimpleMiddleware(BaseMiddleware? next) : base(next) { }

    public override async Task InvokeAsync(HttpContext context)
    {
        context.WriteResponse("<h1>HwGarage is running!</h1>");
        await base.InvokeAsync(context);
    }
}

class Program
{
    static async Task Main()
    {
        var pipeline = new SimpleMiddleware(null);
        var server = new HttpServer("http://localhost:8080/", pipeline);
        await server.StartAsync();
    }
}