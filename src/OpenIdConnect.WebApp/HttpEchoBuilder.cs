using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace OpenIdConnect.WebApp
{
    public static class HttpEchoBuilder
    {
        public static async Task Render(HttpRequest request, TextWriter writer)
        {
            request.EnableBuffering();
            string queryString = request.QueryString.HasValue ? $"?{request.QueryString}" : "";
            await writer.WriteLineAsync($"{request.Method} {request.Path}{queryString} {request.Protocol}");
            foreach (var header in request.Headers)
            {
                await writer.WriteLineAsync($"{header.Key}: {header.Value}");
            }
            using var buffer = new MemoryStream(0x10000);
            await request.Body.CopyToAsync(buffer);
            string body = Encoding.UTF8.GetString(buffer.ToArray());
            await writer.WriteLineAsync();
            await writer.WriteLineAsync(body);
            await writer.FlushAsync();
        }
    }
}
