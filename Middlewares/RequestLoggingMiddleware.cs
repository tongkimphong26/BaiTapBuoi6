using Microsoft.AspNetCore.Http;
using System.Diagnostics;

namespace BookManagement.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var startTime = DateTime.Now;

            var method = context.Request.Method;
            var path = context.Request.Path;

            var stopwatch = Stopwatch.StartNew();

            Console.WriteLine(
                $"[{startTime:yyyy-MM-dd HH:mm:ss}] " +
                $"Method: {method} - Path: {path}"
            );

            if (path == "/Books/Details/0" ||
                path == "/Books/Details/-1")
            {
                context.Response.StatusCode = 400;
                context.Response.ContentType =
                    "text/plain; charset=utf-8";

                await context.Response.WriteAsync(
                    "Book id không hợp lệ"
                );

                stopwatch.Stop();

                Console.WriteLine(
                    $"Status Code: {context.Response.StatusCode} " +
                    $"- Time: {stopwatch.ElapsedMilliseconds} ms"
                );

                return;
            }

            await _next(context);

            stopwatch.Stop();

            Console.WriteLine(
                $"Status Code: {context.Response.StatusCode} " +
                $"- Time: {stopwatch.ElapsedMilliseconds} ms"
            );
        }
    }
}