using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebSoftMast_02.Pages.Users
{
    [Authorize(Roles="Admins")]
    public class AdminPageModel : PageModel
    {

    }

}
