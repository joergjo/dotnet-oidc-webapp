using System.Text;

namespace AadClaimsViewer
{
    public static class HttpRequestExtensions
    {
        public static async Task WriteAsync(this HttpRequest request, TextWriter writer)
        {
            request.EnableBuffering();
            var queryString = request.QueryString.HasValue ? $"?{request.QueryString}" : "";
            await writer.WriteLineAsync($"{request.Method} {request.Path}{queryString} {request.Protocol}");
            foreach (var header in request.Headers)
            {
                await writer.WriteLineAsync($"{header.Key}: {header.Value}");
            }
            using var buffer = new MemoryStream(0x10000);
            await request.Body.CopyToAsync(buffer);
            var body = Encoding.UTF8.GetString(buffer.ToArray());
            await writer.WriteLineAsync();
            await writer.WriteLineAsync(body);
            await writer.FlushAsync();
        }
    }
}
