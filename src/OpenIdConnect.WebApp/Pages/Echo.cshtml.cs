﻿using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace OpenIdConnect.WebApp.Pages
{
    [AllowAnonymous]
    public class EchoModel : PageModel
    {
        private readonly ILogger _logger;

        public string RequestDump { get; set; }

        public EchoModel(ILogger<EchoModel> logger)
        {
            _logger = logger;
        }

        public async Task OnGet()
        {
            using var writer = new StringWriter();
            await HttpEchoBuilder.Render(Request, writer);
            RequestDump = writer.ToString();
        }
    }
}
