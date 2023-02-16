using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AadClaimsViewer.Pages
{
    public class ClaimsModel : PageModel
    {
        public IEnumerable<Claim> Claims { get; set; } = Array.Empty<Claim>();

        public void OnGet()
        {
            Claims = User.Claims.OrderBy(c => c.Type);
        }
    }
}
