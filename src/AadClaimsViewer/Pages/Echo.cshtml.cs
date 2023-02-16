using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AadClaimsViewer.Pages
{
    [AllowAnonymous]
    public class EchoModel : PageModel
    {
        private readonly ILogger _logger;

        public string? RequestDump { get; set; }

        public EchoModel(ILogger<EchoModel> logger)
        {
            _logger = logger;
        }

        public async Task OnGet()
        {
            using var writer = new StringWriter();
            _logger.LogDebug("Dumping request");
            await Request.WriteAsync(writer);
            RequestDump = writer.ToString();
        }
    }
}
