using Microsoft.AspNetCore.Authorization;

namespace AadClaimsViewer
{
    public static class WebApplicationExtensions
    {
        public static void MapProbes(
            this WebApplication app,
            int probePort)
        {
            if (app is null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            var host = $"*:{(probePort > 0 ? $"{probePort}" : "*")}";
            var allowAnonymous = new AllowAnonymousAttribute();

            app
                .MapHealthChecks("healthz/ready")
                .RequireHost(host)
                .WithMetadata(allowAnonymous);
            app
                .MapHealthChecks("healthz/live")
                .RequireHost(host)
                .WithMetadata(allowAnonymous);
        }
    }
}
