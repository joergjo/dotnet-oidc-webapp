using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EasyAuth.WebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private const string VisitsKey = $"{nameof(IndexModel)}.Visits";

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            var welcomeText = "Welcome back!";
            var visits = HttpContext.Session.GetInt32(VisitsKey);
            if (visits is null)
            {
                welcomeText = "Welcome for the first time!";
                visits = 0;
            }

            ViewData["Welcome"] = welcomeText;
            _logger.LogInformation("Number of visits: {Visits}", visits);
            visits += 1;
            HttpContext.Session.SetInt32(VisitsKey, visits!.Value);
        }
    }
}
