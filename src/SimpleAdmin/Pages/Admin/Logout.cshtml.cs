using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SimpleAdmin.Pages.Admin
{
    public class LogoutModel(SignInManager<ApplicationUser> signInManager) : PageModel
    {
        public async Task<IActionResult> OnGetAsync()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                await signInManager.SignOutAsync();
            }
            return Redirect("/admin/login");

        }
    }
}
