using System;
using System.Collections.Generic;
using System.Net;

namespace HwGarage.Core.Http
{
    public class HttpContext
    {
        public HttpListenerContext RawContext { get; }
        public HttpListenerRequest Request => RawContext.Request;
        public HttpListenerResponse Response => RawContext.Response;
        
        // Можно хранить информацию о пользователе, если авторизован
        public object? User { get; set; }

        // Кэш для данных между middleware
        public Dictionary<string, object> Items { get; } = new();

        public HttpContext(HttpListenerContext context)
        {
            RawContext = context ?? throw new ArgumentNullException(nameof(context));
        }

        // Удобный метод для ответа
        public void WriteResponse(string content, string contentType = "text/html", int statusCode = 200)
        {
            var buffer = System.Text.Encoding.UTF8.GetBytes(content);
            Response.StatusCode = statusCode;
            Response.ContentType = contentType;
            Response.ContentLength64 = buffer.Length;
            using var output = Response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
        }
    }
}